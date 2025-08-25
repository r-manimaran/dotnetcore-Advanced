using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopHub.Contracts.Models;

namespace ShopHub.WebApi.Data.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        
        builder.Property(o => o.Id)
            .HasMaxLength(50)
            .IsRequired();
            
        builder.Property(o => o.UserId)
            .HasMaxLength(50)
            .IsRequired();
            
        builder.Property(o => o.Currency)
            .HasMaxLength(3)
            .HasDefaultValue("USD")
            .IsRequired();
            
        builder.Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
            
        builder.Property(o => o.PaymentStatus)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
            
        builder.Property(o => o.CreatedOnUtc)
            .IsRequired();
            
        builder.Ignore(o => o.TotalAmount);
        
        builder.OwnsMany(o => o.Items, item =>
        {
            item.Property(i => i.ProductId)
                .HasMaxLength(50)
                .IsRequired();
                
            item.Property(i => i.ProductName)
                .HasMaxLength(200)
                .IsRequired();
                
            item.Property(i => i.Quantity)
                .IsRequired();
                
            item.Property(i => i.UnitPrice)
                .HasPrecision(18, 2)
                .IsRequired();
        });
        
        builder.HasIndex(o => o.UserId);
        builder.HasIndex(o => o.Status);
        builder.HasIndex(o => o.CreatedOnUtc);
    }
}
