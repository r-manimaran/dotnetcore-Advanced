using Microsoft.EntityFrameworkCore;
using ShopHub.Contracts.Models;

namespace ShopHub.WebApi.Data.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
            .HasMaxLength(50)
            .IsRequired();
            
        builder.Property(p => p.Name)
            .HasMaxLength(200)
            .IsRequired();
            
        builder.Property(p => p.Description)
            .HasMaxLength(1000);
            
        builder.Property(p => p.Category)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
            
        builder.Property(p => p.Price)
            .HasPrecision(18, 2)
            .IsRequired();
            
        builder.Property(p => p.StockQuantity)
            .IsRequired();
            
        builder.Property(p => p.Currency)
            .HasMaxLength(3)
            .IsRequired();
            
        builder.Property(p => p.IsActive)
            .HasDefaultValue(true);
            
        builder.Property(p => p.CreateOnUtc)
            .IsRequired();
            
        builder.HasIndex(p => p.Name);
        builder.HasIndex(p => p.Category);
        builder.HasIndex(p => p.IsActive);
    }
}
