using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApi.Migrations.Products
{
    /// <inheritdoc />
    public partial class createproductsdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "products");

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "products",
                table: "Products",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Product #1", 100m },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Product #2", 200m },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Product #3", 300m },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Product #4", 400m },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "Product #5", 500m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products",
                schema: "products");
        }
    }
}
