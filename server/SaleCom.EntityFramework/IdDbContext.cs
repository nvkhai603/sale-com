using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SaleCom.Domain.Tenants;
using SaleCom.Domain.Identity;
using Nvk.Data;
using SaleCom.Domain.Licenses;

namespace SaleCom.EntityFramework
{
    public class IdDbContext : AppDbContext<IdDbContext>
    {
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<TenantUser> TenantUsers { get; set; }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<AppRole> Roles { get; set; }
        public DbSet<AppUserClaim> UserClaims { get; set; }
        public DbSet<AppUserRole> UserRoles { get; set; }
        public DbSet<IdentityUserLogin<Guid>> UserLogins { get; set; }
        public DbSet<IdentityUserToken<Guid>> UserTokens { get; set; }
        public DbSet<IdentityRoleClaim<Guid>> RoleClaims { get; set; }
        public DbSet<AppAuthenticationTicket> AuthenticationTickets { get; set; }
        public DbSet<LicensePack> LicensePacks { get; set; }
        public DbSet<DomainLicense> DomainLicenses { get; set; }
        public DbSet<DomainTenant> TenantDomains { get; set; }

        public IdDbContext(DbContextOptions<IdDbContext> options, ILazyServiceProvider lazyServiceProvider) : base(options, lazyServiceProvider)
        {
        }
        private StoreOptions GetStoreOptions() => this.GetService<IDbContextOptions>()
                            .Extensions.OfType<CoreOptionsExtension>()
                            .FirstOrDefault()?.ApplicationServiceProvider
                            ?.GetService<IOptions<IdentityOptions>>()
                            ?.Value?.Stores;

        private class PersonalDataConverter : ValueConverter<string, string>
        {
            public PersonalDataConverter(IPersonalDataProtector protector) : base(s => protector.Protect(s), s => protector.Unprotect(s), default)
            { }
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Configure default schema
            base.OnModelCreating(builder);
            builder.Entity<Tenant>(b => {
                b.HasKey(t => t.Id);
                b.HasIndex(t => t.Name).HasDatabaseName("TenantNameIndex");
                b.ToTable("tenants");
            });
            builder.Entity<TenantUser>(b =>
            {
                b.ToTable("tenant_users");
                b.HasKey(tu => new { tu.TenantId, tu.UserId });
            });
            //TODO
            var storeOptions = GetStoreOptions();
            var maxKeyLength = storeOptions?.MaxLengthForKeys ?? 0;
            var encryptPersonalData = storeOptions?.ProtectPersonalData ?? false;
            PersonalDataConverter converter = null;

            builder.Entity<AppUser>(b =>
            {
                b.HasKey(u => u.Id);
                b.HasIndex(u => u.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique();
                b.HasIndex(u => u.NormalizedEmail).HasDatabaseName("EmailIndex");
                b.ToTable("users");
                b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

                b.Property(u => u.UserName).HasMaxLength(256);
                b.Property(u => u.NormalizedUserName).HasMaxLength(256);
                b.Property(u => u.Email).HasMaxLength(256);
                b.Property(u => u.NormalizedEmail).HasMaxLength(256);

                if (encryptPersonalData)
                {
                    converter = new PersonalDataConverter(this.GetService<IPersonalDataProtector>());
                    var personalDataProps = typeof(AppUser).GetProperties().Where(
                                    prop => Attribute.IsDefined(prop, typeof(ProtectedPersonalDataAttribute)));
                    foreach (var p in personalDataProps)
                    {
                        if (p.PropertyType != typeof(string))
                        {
                            throw new InvalidOperationException("CanOnlyProtectStrings");
                        }
                        b.Property(typeof(string), p.Name).HasConversion(converter);
                    }
                }

                b.HasMany<AppUserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
                b.HasMany<IdentityUserLogin<Guid>>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
                b.HasMany<IdentityUserToken<Guid>>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
                b.HasMany<AppUserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
            });

            builder.Entity<AppUserClaim>(b =>
            {
                b.HasKey(uc => uc.Id);
                b.ToTable("user_claims");
            });

            builder.Entity<IdentityUserLogin<Guid>>(b =>
            {
                b.HasKey(l => new { l.LoginProvider, l.ProviderKey });

                if (maxKeyLength > 0)
                {
                    b.Property(l => l.LoginProvider).HasMaxLength(maxKeyLength);
                    b.Property(l => l.ProviderKey).HasMaxLength(maxKeyLength);
                }

                b.ToTable("user_logins");
            });

            builder.Entity<IdentityUserToken<Guid>>(b =>
            {
                b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

                if (maxKeyLength > 0)
                {
                    b.Property(t => t.LoginProvider).HasMaxLength(maxKeyLength);
                    b.Property(t => t.Name).HasMaxLength(maxKeyLength);
                }

                if (encryptPersonalData)
                {
                    var tokenProps = typeof(AppUser).GetProperties().Where(
                                    prop => Attribute.IsDefined(prop, typeof(ProtectedPersonalDataAttribute)));
                    foreach (var p in tokenProps)
                    {
                        if (p.PropertyType != typeof(string))
                        {
                            throw new InvalidOperationException("CanOnlyProtectStrings");
                        }
                        b.Property(typeof(string), p.Name).HasConversion(converter);
                    }
                }

                b.ToTable("user_tokens");
            });

            builder.Entity<AppRole>(b =>
            {
                b.HasKey(r => r.Id);
                b.HasIndex(r => r.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique();
                b.ToTable("roles");
                b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

                b.Property(u => u.Name).HasMaxLength(256);
                b.Property(u => u.NormalizedName).HasMaxLength(256);

                b.HasMany<AppUserRole>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();
                b.HasMany<IdentityRoleClaim<Guid>>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
            });

            builder.Entity<IdentityRoleClaim<Guid>>(b =>
            {
                b.HasKey(rc => rc.Id);
                b.ToTable("role_claims");
            });

            builder.Entity<AppUserRole>(b =>
            {
                b.HasKey(r => new { r.UserId, r.RoleId });
                b.ToTable("user_roles");
            });

            builder.Entity<AppAuthenticationTicket>(b =>
            {
                b.HasKey(a => new { a.Id });
                b.HasIndex(a => a.UserId);
                b.ToTable("authentication_tickets");
            });

            builder.Entity<LicensePack>(b =>
            {
                b.HasKey(a => new { a.Id });
                b.ToTable("license_packs");
            });

            builder.Entity<DomainLicense>(b =>
            {
                b.HasKey(a => new { a.Id });
                b.ToTable("domain_licenses");
            });
            builder.Entity<DomainTenant>(b =>
            {
                b.HasKey(a => new { a.OwnerId });
                b.ToTable("domain_tenants");
            });
        }
    }
}
