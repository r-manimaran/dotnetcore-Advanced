using Bogus;
using Microsoft.EntityFrameworkCore;
using ShopHub.Contracts.Enums;
using ShopHub.Contracts.Models;

namespace ShopHub.WebApi.Data;

public static class DatabaseSeedService
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Products.AnyAsync())
        {
            return;
        }
        
        var products = GenerateProducts();
        context.Products.AddRange(products);
        await context.SaveChangesAsync();
    }

    private static List<Product> GenerateProducts()
    {
        var products = new List<Product>();
        var categories = Enum.GetValues<ProductCategory>();
        
        foreach (var category in categories)
        {
            var categoryProducts = new Faker<Product>()
                .RuleFor(p => p.Id, f => Guid.NewGuid().ToString())
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Category, category)
                .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price()))
                .RuleFor(p => p.StockQuantity, f => f.Random.Int(1, 100))
                .RuleFor(p => p.Currency, "USD")
                .RuleFor(p => p.IsActive, true)
                .RuleFor(p => p.CreateOnUtc, DateTime.UtcNow)
                .Generate(10);
                
            products.AddRange(categoryProducts);
        }
        
        return products;
    }
}
