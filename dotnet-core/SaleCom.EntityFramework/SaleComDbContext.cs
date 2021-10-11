using Microsoft.EntityFrameworkCore;
using SaleCom.Domain.Shared.Orders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SaleCom.EntityFramework
{
    public class SaleComDbContext : DbContext
    {
        public DbSet<Order> Order;
        public SaleComDbContext([NotNullAttribute] DbContextOptions<SaleComDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure default schema
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>().ToTable("orders");
        }
    }
}
