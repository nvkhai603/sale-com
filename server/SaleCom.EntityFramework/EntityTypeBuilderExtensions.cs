using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nvk.Data;
using Nvk.Ddd.Domain;
using Nvk.MultiTenancy;
using Nvk.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaleCom.EntityFramework
{
    public static class EntityTypeBuilderExtensions
    {
        public static void ConfigureByConvention(this EntityTypeBuilder b)
        {
            b.TryConfigureConcurrencyStamp();
            b.TryConfigureSoftDelete();
            b.TryConfigureMultiTenant();
            b.TryConfigureMustHaveCurrentUser();
            b.TryConfigureCreateAndModified();
        }
        public static void ConfigureCreateAndModified<T>(this EntityTypeBuilder<T> b) 
            where T : class, IEntity
        {
            b.As<EntityTypeBuilder>();
        }
        public static void TryConfigureCreateAndModified(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<IEntity>())
            {
                b.Property(nameof(IEntity.CreationTime))
                    .HasColumnName(nameof(IEntity.CreationTime));
                b.Property(nameof(IEntity.CreatorId))
                    .HasColumnName(nameof(IEntity.CreatorId));
                b.Property(nameof(IEntity.LastModificationTime))
                    .HasColumnName(nameof(IEntity.LastModificationTime));
                b.Property(nameof(IEntity.LastModifierId))
                    .HasColumnName(nameof(IEntity.LastModifierId));
            }
        }
        public static void ConfigureConcurrencyStamp<T>(this EntityTypeBuilder<T> b)
            where T : class, IHasConcurrencyStamp
        {
            b.As<EntityTypeBuilder>().TryConfigureConcurrencyStamp();
        }
        public static void TryConfigureConcurrencyStamp(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<IHasConcurrencyStamp>())
            {
                b.Property(nameof(IHasConcurrencyStamp.ConcurrencyStamp))
                    .IsConcurrencyToken()
                    .HasMaxLength(ConcurrencyStampConsts.MaxLength)
                    .HasColumnName(nameof(IHasConcurrencyStamp.ConcurrencyStamp));
            }
        }
        public static void ConfigureSoftDelete<T>(this EntityTypeBuilder<T> b)
            where T : class, ISoftDelete
        {
            b.As<EntityTypeBuilder>().TryConfigureSoftDelete();
        }
        public static void TryConfigureSoftDelete(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<ISoftDelete>())
            {
                b.Property(nameof(ISoftDelete.IsDeleted))
                    .IsRequired().HasDefaultValue(false);
                b.Property(nameof(ISoftDelete.DeletionTime));
                b.Property(nameof(ISoftDelete.DeletedId));
            }
        }
        public static void ConfigureMultiTenant<T>(this EntityTypeBuilder<T> b)
            where T : class, IMultiTenant
        {
            b.As<EntityTypeBuilder>().TryConfigureMultiTenant();
        }
        public static void TryConfigureMultiTenant(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<IMultiTenant>())
            {
                b.Property(nameof(IMultiTenant.TenantId))
                    .IsRequired(false)
                    .HasColumnName(nameof(IMultiTenant.TenantId));
            }
        }

        public static void TryConfigureMustHaveCurrentUser(this EntityTypeBuilder b)
        {
            if (b.Metadata.ClrType.IsAssignableTo<IMustHaveCurrentUser>())
            {
                b.Property(nameof(IMustHaveCurrentUser.UserId))
                    .IsRequired(true)
                    .HasColumnName(nameof(IMustHaveCurrentUser.UserId));
            }
        }
    }
}
