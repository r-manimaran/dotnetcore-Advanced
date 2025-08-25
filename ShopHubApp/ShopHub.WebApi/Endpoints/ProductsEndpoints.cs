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
            var result = await productService.GetAllAsync();
            return result.Success ? Results.Ok(result.Data) : Results.Problem(result.ErrorMessage);
        });

        app.MapGet("/products/{id}", async (string id, IProductService productService) =>
        {
            var result = await productService.GetByIdAsync(id);
            return result.Success ? Results.Ok(result.Data):Results.NotFound(result.ErrorMessage);

        });

        app.MapGet("/products/category/{category}", async (ProductCategory category, IProductService productService) =>
        {
            var result = await productService.GetByCategoryAsync(category);
            return result.Success ? Results.Ok(result.Data) : Results.Problem(result.ErrorMessage);
        });

        app.MapPost("/products", async (Product product, IProductService productService) =>
        {
            var result = await productService.CreateAsync(product);
            return result.Success ? Results.Created($"/products/{result.Data?.Id}", result.Data) : Results.Problem(result.ErrorMessage);
        });

        app.MapPut("/products/{id}", async (string id, Product product, IProductService productService) =>
        {
            product.Id = id;
            var result = await productService.UpdateAsync(product);
            return result.Success ? Results.NoContent() : Results.NotFound(result.ErrorMessage);
        });

        app.MapDelete("/products/{id}", async (string id, IProductService productService) =>
        {
            var result = await productService.DeleteAsync(id);
            return result.Success? Results.NoContent():Results.NotFound(result.ErrorMessage);
        });
        app.MapPatch("/products/{id}/stock", async (string id, int quantityChange, IProductService productService) =>
        {
            var result = await productService.UpdateStockAsync(id, quantityChange);

            return result.Success ? Results.NoContent() :Results.NotFound(result.ErrorMessage);
        });
    }
}
