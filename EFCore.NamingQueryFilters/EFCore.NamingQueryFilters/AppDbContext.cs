using EFCore.NamingQueryFilters.Model;
using Microsoft.EntityFrameworkCore;

namespace EFCore.NamingQueryFilters;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
                    .HasQueryFilter(ProductFilters.ActiveFilter, p=> p.IsActive) 
                    .HasQueryFilter(ProductFilters.PriceFilter, p=>p.Price >10); 
               

    }
}

public static class ProductFilters
{
    public const string ActiveFilter = "ActiveFilter";
    public const string PriceFilter = "PriceFilter";
}