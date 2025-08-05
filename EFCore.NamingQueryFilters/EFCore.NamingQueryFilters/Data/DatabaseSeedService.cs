using Bogus;
using EFCore.NamingQueryFilters.Model;
using Microsoft.EntityFrameworkCore;

namespace EFCore.NamingQueryFilters.Data;

public static class DatabaseSeedService
{
    public static async Task SeedAsync(AppDbContext dbContext)
    {
        if (await dbContext.Products.AnyAsync())
            return;

        var products = GenerateProducts(25);

        await dbContext.Products.AddRangeAsync(products);
        await dbContext.SaveChangesAsync();

    }

    private static List<Product> GenerateProducts(int count=50)
    {
        return new Faker<Product>()
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.IsActive, f => f.Random.Bool(0.8f)) // 80% chance to be active                                                                
            .RuleFor(p => p.Price, f => f.Random.Decimal(1.0m, 100.0m))
            .RuleFor(p => p.CreatedAt, f => f.Date.Past(1)) // Created within the last year
            .Generate(count);            
    }
}
