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
        modelBuilder.Entity<Product>().HasQueryFilter(p => p.IsActive);
    }
}
