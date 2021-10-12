using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SaleCom.Domain.Shared.Identity;
using SaleCom.Domain.Shared.Tenants;
using System;

namespace SaleCom.EntityFramework
{
    public class IdDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public DbSet<Tenant> Tenant { get; set; }
        public DbSet<TenantUser> UserTenant { get; set; }
        public IdDbContext(DbContextOptions<IdDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure default schema
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AppUser>().ToTable("users");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("user_tokens");
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins");
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims");
            modelBuilder.Entity<AppRole>().ToTable("roles");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("user_roles");
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("role_claims");
            modelBuilder.Entity<Tenant>().ToTable("tenants");
            modelBuilder.Entity<TenantUser>().ToTable("tenant_users").HasNoKey();
        }
    }
}
