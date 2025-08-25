using Carter;
using ShopHub.Contracts.Enums;
using ShopHub.Contracts.Models;
using ShopHub.WebApi.Services;

namespace ShopHub.WebApi.Endpoints;

public class ProductsEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (IProductService productService) =>
        {

        });

        app.MapGet("/products/{id}", async (string id, IProductService productService) =>
        {

        });

        app.MapGet("/products/category/{category}", async (ProductCategory category, IProductService productService) =>
        {

        });

        app.MapPost("/products", async (Product product, IProductService productService) =>
        {

        });

        app.MapPut("/products/{id}", async (string id, Product product, IProductService productService) =>
        {

        });

        app.MapDelete("/products/{id}", async (string id, IProductService productService) =>
        {

        });
        app.MapPatch("/products/{id}/stock", async (string id, int quantityChange, IProductService productService) =>
        {

        });
    }
}
