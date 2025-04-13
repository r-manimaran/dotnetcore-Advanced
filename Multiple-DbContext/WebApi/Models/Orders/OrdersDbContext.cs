using Microsoft.EntityFrameworkCore;

using WebApi.Models.Orders;

namespace WebApi.Models;

public class OrdersDbContext: DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options): base(options)
    {
        
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<LineItem> LineItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("orders");

        modelBuilder.Entity<Order>().HasKey(x => x.Id);

        modelBuilder.Entity<Order>().HasMany(o => o.LineItems).WithOne().HasForeignKey(li=>li.OrderId);

    }
}
