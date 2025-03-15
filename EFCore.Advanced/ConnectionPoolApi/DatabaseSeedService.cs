using Bogus;
using Microsoft.EntityFrameworkCore;
using ProductCatalogDb;

namespace ConnectionPoolApi;

public class DatabaseSeedService
{
    public static async Task SeedDatabase(AppDbContext context)
    {
        if(await context.Products.AnyAsync())
        {
            return;
        }

        var products = GenerateProducts(100);
        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }

    public static List<Product> GenerateProducts(int count)
    {
        return new Faker<Product>()
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Quantity, f => f.Random.Int(50, 200))
            .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price()))
            .Generate(count);
    }
}
