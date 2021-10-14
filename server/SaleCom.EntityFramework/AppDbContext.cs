using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nvk.Ddd.Domain;
using Nvk.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SaleCom.EntityFramework
{
    public class AppDbContext<TDbContext> : DbContext where TDbContext : DbContext
    {
        public AppDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {

        }

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

            if (!typeof(IEntity).IsAssignableFrom(typeof(TEntity)))
            {
                return;
            }
            modelBuilder.Entity<TEntity>().ConfigureByConvention();
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
        }

        protected virtual void SetModifierProperties(EntityEntry entry) {
            var entity = entry.Entity as IEntity;
            if (entity == null)
            {
                return;
            }

            Entry(entity).Property(x => x.LastModificationTime).OriginalValue = entity.LastModificationTime;
            entity.LastModificationTime = DateTime.Now;
        }
        #endregion
    }
}
