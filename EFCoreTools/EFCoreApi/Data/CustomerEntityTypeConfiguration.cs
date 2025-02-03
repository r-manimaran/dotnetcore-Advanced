using EFCoreApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreApi.Data
{
    public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(x=>x.Id);

            builder.Property(x=>x.Name).IsRequired().HasMaxLength(100);

            builder.Property(x=> x.Email).IsRequired().HasMaxLength(100);

        }
    }
}
