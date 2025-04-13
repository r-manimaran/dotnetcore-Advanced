using Microsoft.EntityFrameworkCore;

namespace WebApi.Models.Products;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) :base(options)
    {
        
    }
    public DbSet<Product> Products { get; set; }

    // For Read Replica scenario
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    //}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("products");

        modelBuilder.Entity<Product>().HasData(SeedProducts);
    }

    private static readonly Product[] SeedProducts =
    {
        // new() { Id =Guid.NewGuid(), Name="Product #1", Price=100m},
        // new() { Id =Guid.NewGuid(), Name="Product #2", Price=200m},
        // new() { Id =Guid.NewGuid(), Name="Product #3", Price=300m},
        // new() { Id =Guid.NewGuid(), Name="Product #4", Price=400m},
        // new() { Id =Guid.NewGuid(), Name="Product #5", Price=500m}

        new() { Id = new Guid("11111111-1111-1111-1111-111111111111"), Name="Product #1", Price=100m},
        new() { Id = new Guid("22222222-2222-2222-2222-222222222222"), Name="Product #2", Price=200m},
        new() { Id = new Guid("33333333-3333-3333-3333-333333333333"), Name="Product #3", Price=300m},
        new() { Id = new Guid("44444444-4444-4444-4444-444444444444"), Name="Product #4", Price=400m},
        new() { Id = new Guid("55555555-5555-5555-5555-555555555555"), Name="Product #5", Price=500m}

    };
}
