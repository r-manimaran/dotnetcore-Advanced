using EFCore.NamingQueryFilters.Model;
using Microsoft.EntityFrameworkCore;

namespace EFCore.NamingQueryFilters.Endpoints;

public static class ProductsEndpoints
{
    public static void MapProductsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (AppDbContext db) =>
        {
            return await db.Products.ToListAsync();
        })
        .WithName("GetAllProducts")
        .WithTags("Products");


        app.MapGet("/products/{id}", async (int id, AppDbContext db) =>
        {
            return await db.Products.FindAsync(id)
                is Product product
                    ? Results.Ok(product)
                    : Results.NotFound();
        })
        .WithName("GetProductById")
        .WithTags("Products");
    }
}
