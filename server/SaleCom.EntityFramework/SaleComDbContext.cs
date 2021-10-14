using Microsoft.EntityFrameworkCore;
using SaleCom.Domain.Shared.Orders;
using SaleCom.Domain.Shared.Products;
using SaleCom.Domain.Shared.Varations;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SaleCom.EntityFramework
{
    public class SaleComDbContext : AppDbContext<SaleComDbContext>
    {
        public DbSet<Product> Product { get; set; }
        public DbSet<Varation> Varation { get; set; }
        public DbSet<Order> Order { get; set; }


        public SaleComDbContext([NotNullAttribute] DbContextOptions<SaleComDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure default schema
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().ToTable("products");
            modelBuilder.Entity<Varation>().ToTable("varations");
            modelBuilder.Entity<Order>().ToTable("orders");
        }
    }
}
