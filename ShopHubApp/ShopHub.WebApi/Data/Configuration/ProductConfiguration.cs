using Microsoft.EntityFrameworkCore;
using ShopHub.Contracts.Models;

namespace ShopHub.WebApi.Data.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Product> builder)
    {
        throw new NotImplementedException();
    }
}
