﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SaleCom.EntityFramework;

namespace SaleCom.EntityFramework.Migrations.SaleComDb
{
    [DbContext(typeof(SaleComDbContext))]
    partial class SaleComDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.10");

            modelBuilder.Entity("SaleCom.Domain.Shared.Orders.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Code")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("orders");
                });
#pragma warning restore 612, 618
        }
    }
}
