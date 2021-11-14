using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Nvk.Data;
using Nvk.Ddd.Domain;
using Nvk.MultiTenancy;
using Nvk.Utilities;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SaleCom.EntityFramework
{
    public class AppDbContext<TDbContext> : DbContext where TDbContext : DbContext
    {
        private readonly ILazyServiceProvider _lazyServiceProvider;
        public AppDbContext([NotNullAttribute] DbContextOptions options, ILazyServiceProvider lazyServiceProvider) : base(options)
        {
            _lazyServiceProvider = lazyServiceProvider;
        }

        protected ICurrentUser _currentUser => _lazyServiceProvider.LazyGetService<ICurrentUser>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                ConfigureBasePropertiesMethodInfo
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] { modelBuilder, entityType });

                ConfigureValueConverterMethodInfo
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] { modelBuilder, entityType });

                ConfigureValueGeneratedMethodInfo
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] { modelBuilder, entityType });
            }
        }

        private static readonly MethodInfo ConfigureBasePropertiesMethodInfo
            = typeof(AppDbContext<TDbContext>)
                .GetMethod(
                    nameof(ConfigureBaseProperties),
                    BindingFlags.Instance | BindingFlags.NonPublic
                );
        private static readonly MethodInfo ConfigureValueConverterMethodInfo
            = typeof(AppDbContext<TDbContext>)
                .GetMethod(
                    nameof(ConfigureValueConverter),
                    BindingFlags.Instance | BindingFlags.NonPublic
                );
        private static readonly MethodInfo ConfigureValueGeneratedMethodInfo
            = typeof(AppDbContext<TDbContext>)
                .GetMethod(
                    nameof(ConfigureValueGenerated),
                    BindingFlags.Instance | BindingFlags.NonPublic
                );

        protected virtual void ConfigureBaseProperties<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
            where TEntity : class
        {
            if (mutableEntityType.IsOwned())
            {
                return;
            }

            if (!typeof(IShouldConfigureBaseProperties).IsAssignableFrom(typeof(TEntity)))
            {
                return;
            }
            modelBuilder.Entity<TEntity>().ConfigureByConvention();
            modelBuilder.ConfigureDecimalPrecisionAndScale();
            ConfigureGlobalFilters<TEntity>(modelBuilder, mutableEntityType);
        }
        protected virtual void ConfigureValueConverter<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
            where TEntity : class
        {
            //TODO
            return;
        }
        protected virtual void ConfigureValueGenerated<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
            where TEntity : class
        {
            if (!typeof(IEntity<Guid>).IsAssignableFrom(typeof(TEntity)))
            {
                return;
            }

            var idPropertyBuilder = modelBuilder.Entity<TEntity>().Property(x => ((IEntity<Guid>)x).Id);
            if (idPropertyBuilder.Metadata.PropertyInfo.IsDefined(typeof(DatabaseGeneratedAttribute), true))
            {
                return;
            }

            idPropertyBuilder.ValueGeneratedNever();
        }

        /// <summary>
        /// This method will call the DbContext <see cref="SaveChangesAsync(bool, CancellationToken)"/> method directly of EF Core, which doesn't apply concepts of abp.
        /// </summary>
        public virtual Task<int> SaveChangesOnDbContextAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            try
            {
                ApplyConcepts();
                var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
                //TODO: Domain event, history, auditLog
                return result;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                //TODO: Logginng
                throw ex;
            }
            finally
            {
                ChangeTracker.AutoDetectChangesEnabled = true;
            }
        }
        protected virtual void ApplyConcepts()
        {
            foreach (var entry in ChangeTracker.Entries().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        ApplyConceptsForAddedEntity(entry);
                        break;
                    case EntityState.Modified:
                        ApplyConceptsForModifiedEntity(entry);
                        break;
                    case EntityState.Deleted:
                        ApplyConceptsForDeletedEntity(entry);
                        break;
                }
                //TODO: Domain event
            }
        }
        protected virtual void ApplyConceptsForAddedEntity(EntityEntry entry)
        {
            CheckAndSetId(entry);
            SetConcurrencyStampIfNull(entry);
            SetCreatorProperties(entry);
            SetTenantId(entry);
            SetWareHouseId(entry);
        }
        protected virtual void ApplyConceptsForModifiedEntity(EntityEntry entry)
        {
            if (entry.State == EntityState.Modified && entry.Properties.Any(x => x.IsModified && x.Metadata.ValueGenerated == ValueGenerated.Never))
            {
                UpdateConcurrencyStamp(entry);
                SetModifierProperties(entry);
            }
        }
        protected virtual void ApplyConceptsForDeletedEntity(EntityEntry entry)
        {
            if (TryCancelDeletionForSoftDelete(entry))
            {
                UpdateConcurrencyStamp(entry);
                //TODO: SetAuditProperties
            }
        }

        #region ApplyConceptFunction
        protected virtual void CheckAndSetId(EntityEntry entry)
        {
            if (entry.Entity is IEntity<Guid> entityWithGuidId)
            {
                TrySetGuidId(entry, entityWithGuidId);
            }
        }
        protected virtual void TrySetGuidId(EntityEntry entry, IEntity<Guid> entity)
        {
            if (entity.Id != default)
            {
                return;
            }

            var idProperty = entry.Property("Id").Metadata.PropertyInfo;

            //Check for DatabaseGeneratedAttribute
            var dbGeneratedAttr = ReflectionHelper
                .GetSingleAttributeOrDefault<DatabaseGeneratedAttribute>(
                    idProperty
                );

            if (dbGeneratedAttr != null && dbGeneratedAttr.DatabaseGeneratedOption != DatabaseGeneratedOption.None)
            {
                return;
            }

            //TODO: Chuyển sang GuidGenerator.Create() đáp ứng nhanh.
            entity.Id = Guid.NewGuid();
        }
        protected virtual void SetConcurrencyStampIfNull(EntityEntry entry)
        {
            var entity = entry.Entity as IHasConcurrencyStamp;
            if (entity == null)
            {
                return;
            }

            if (entity.ConcurrencyStamp != null)
            {
                return;
            }

            entity.ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }
        protected virtual void UpdateConcurrencyStamp(EntityEntry entry)
        {
            var entity = entry.Entity as IHasConcurrencyStamp;
            if (entity == null)
            {
                return;
            }

            Entry(entity).Property(x => x.ConcurrencyStamp).OriginalValue = entity.ConcurrencyStamp;
            entity.ConcurrencyStamp = Guid.NewGuid().ToString("N");
        }
        protected virtual bool TryCancelDeletionForSoftDelete(EntityEntry entry)
        {
            if (!(entry.Entity is ISoftDelete))
            {
                return false;
            }
            entry.Reload();
            entry.State = EntityState.Modified;
            entry.Entity.As<ISoftDelete>().IsDeleted = true;
            entry.Entity.As<ISoftDelete>().DeletionTime = DateTime.Now;
            return true;
        }
        protected virtual void SetCreatorProperties(EntityEntry entry)
        {
            var entity = entry.Entity as IEntity;
            if (entity == null)
            {
                return;
            }

            Entry(entity).Property(x => x.CreationTime).OriginalValue = entity.CreationTime;
            entity.CreationTime = DateTime.Now;

            Entry(entity).Property(x => x.CreatorId).OriginalValue = entity.CreatorId;
            entity.CreatorId = _currentUser.Id;
        }

        protected virtual void SetTenantId(EntityEntry entry)
        {
            var entity = entry.Entity as IMultiTenant;
            if (entity == null)
            {
                return;
            }
            Entry(entity).Property(x => x.TenantId).OriginalValue = entity.TenantId;
            entity.TenantId = _currentUser.TenantId;
        }

        protected virtual void SetWareHouseId(EntityEntry entry)
        {
            var entity = entry.Entity as IMultiWareHouse;
            if (entity == null)
            {
                return;
            }
            Entry(entity).Property(x => x.WareHouseId).OriginalValue = entity.WareHouseId;
            entity.WareHouseId = _currentUser.WareHouseId;
        }

        protected virtual void SetModifierProperties(EntityEntry entry)
        {
            var entity = entry.Entity as IEntity;
            if (entity == null)
            {
                return;
            }

            Entry(entity).Property(x => x.LastModificationTime).OriginalValue = entity.LastModificationTime;
            entity.LastModificationTime = DateTime.Now;
        }
        protected virtual void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
            where TEntity : class
        {
            if (mutableEntityType.BaseType == null && ShouldFilterEntity<TEntity>(mutableEntityType))
            {
                var filterExpression = CreateFilterExpression<TEntity>();
                if (filterExpression != null)
                {
                    modelBuilder.Entity<TEntity>().HasQueryFilter(filterExpression);
                }
            }
        }
        protected virtual bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType) where TEntity : class
        {
            if (typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }

            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }

            if (typeof(IMustHaveCurrentUser).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }

            return false;
        }
        protected virtual Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>()
            where TEntity : class
        {
            Expression<Func<TEntity, bool>> expression = null;

            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                expression = e => !EF.Property<bool>(e, "IsDeleted");
            }

            if (typeof(IMultiTenant).IsAssignableFrom(typeof(TEntity)))
            {
                Expression<Func<TEntity, bool>> multiTenantFilter = e => _currentUser.TenantId != null ? EF.Property<Guid>(e, "TenantId").Equals(_currentUser.TenantId) : false; 
                expression = expression == null ? multiTenantFilter : CombineExpressions(expression, multiTenantFilter);
            }

            if (typeof(IMultiWareHouse).IsAssignableFrom(typeof(TEntity)))
            {
                Expression<Func<TEntity, bool>> multiWareHouseFilter = e => _currentUser.WareHouseId != null ? EF.Property<Guid>(e, "WareHouseId").Equals(_currentUser.WareHouseId) : false;
                expression = expression == null ? multiWareHouseFilter : CombineExpressions(expression, multiWareHouseFilter);
            }

            if (typeof(IMustHaveCurrentUser).IsAssignableFrom(typeof(TEntity)))
            {
                Expression<Func<TEntity, bool>> currentUserFilter = e => EF.Property<Guid>(e, "UserId").Equals(_currentUser.Id); // TODO
                expression = expression == null ? currentUserFilter : CombineExpressions(expression, currentUserFilter);
            }

            return expression;
        }
        protected virtual Expression<Func<T, bool>> CombineExpressions<T>(Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expression1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expression1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expression2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expression2.Body);

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left, right), parameter);
        }
        class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                {
                    return _newValue;
                }

                return base.Visit(node);
            }
        }
        #endregion
    }
}
