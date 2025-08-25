using Microsoft.EntityFrameworkCore;
using ShopHub.Contracts.Models;

namespace ShopHub.WebApi.Data;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<User> Users { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

       // modelBuilder.HasDefaultSchema(DatabaseConst.Schema);

       // modelBuilder.UseIdentityByDefaultColumns();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
