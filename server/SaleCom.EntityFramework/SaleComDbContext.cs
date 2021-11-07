using Microsoft.EntityFrameworkCore;
using Nvk.Data;
using SaleCom.Domain.Customers;
using SaleCom.Domain.Orders;
using SaleCom.Domain.Products;
using SaleCom.Domain.Transactions;
using SaleCom.Domain.Varations;
using SaleCom.Domain.WareHouses;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SaleCom.EntityFramework
{
    public class SaleComDbContext : AppDbContext<SaleComDbContext>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Varation> Varations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<MoneyTransaction> MoneyTransactions { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<AdvertisingCost> AdvertisingCosts { get; set; }
        public DbSet<WareHouse> WareHouses { get; set; }

        public SaleComDbContext(DbContextOptions<SaleComDbContext> options, ILazyServiceProvider lazyServiceProvider) : base(options, lazyServiceProvider)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>(b =>
            {
                b.HasKey(p => p.Id);
                b.ToTable("shops");
            })
            ;modelBuilder.Entity<Product>(b =>
            {
                b.HasKey(p => p.Id);
                b.ToTable("products");
            });
            modelBuilder.Entity<Varation>(b =>
            {
                b.HasKey(v => v.Id);
                b.ToTable("varations");
            });
            modelBuilder.Entity<Order>(b =>
            {
                b.HasKey(o => o.Id);
                b.ToTable("orders");
            });
            modelBuilder.Entity<Customer>(b =>
            {
                b.HasKey(o => o.Id);
                b.ToTable("customers");
            });
            modelBuilder.Entity<MoneyTransaction>(b =>
            {
                b.HasKey(o => o.Id);
                b.ToTable("money_transactions");
            });
            modelBuilder.Entity<AdvertisingCost>(b =>
            {
                b.HasKey(o => o.Id);
                b.ToTable("advertising_costs");
            });
            modelBuilder.Entity<WareHouse>(b =>
            {
                b.HasKey(o => o.Id);
                b.ToTable("ware_houses");
            });
        }
    }
}
