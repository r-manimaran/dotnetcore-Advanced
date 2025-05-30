﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApi.Models.Products;

#nullable disable

namespace WebApi.Migrations.Products
{
    [DbContext(typeof(ProductDbContext))]
    partial class ProductDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("products")
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebApi.Models.Products.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Products", "products");

                    b.HasData(
                        new
                        {
                            Id = new Guid("11111111-1111-1111-1111-111111111111"),
                            Name = "Product #1",
                            Price = 100m
                        },
                        new
                        {
                            Id = new Guid("22222222-2222-2222-2222-222222222222"),
                            Name = "Product #2",
                            Price = 200m
                        },
                        new
                        {
                            Id = new Guid("33333333-3333-3333-3333-333333333333"),
                            Name = "Product #3",
                            Price = 300m
                        },
                        new
                        {
                            Id = new Guid("44444444-4444-4444-4444-444444444444"),
                            Name = "Product #4",
                            Price = 400m
                        },
                        new
                        {
                            Id = new Guid("55555555-5555-5555-5555-555555555555"),
                            Name = "Product #5",
                            Price = 500m
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
