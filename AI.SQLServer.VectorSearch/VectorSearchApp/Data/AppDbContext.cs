using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;
using VectorSearchApp.Data.Entities;

namespace VectorSearchApp.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public virtual DbSet<Document> Documents { get; set; }
    public virtual DbSet<DocumentChunk> DocumentChunks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseExceptionProcessor();

        //optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Document>(entity =>
        {
            entity.ToTable("Documents");

            entity.HasKey(d => d.Id);

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                  .IsRequired()
                  .HasMaxLength(255);
        });

        modelBuilder.Entity<DocumentChunk>(entity =>
        {
            entity.ToTable("DocumentChunks");

            entity.HasKey(d => d.Id);

            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.Property(e => e.Content).IsRequired();

            entity.Property(e => e.Embedding)
                  .HasColumnType("vector(1536)")
                  .IsRequired();

            entity.HasOne(d => d.Document).WithMany(p => p.Chunks)
                  .HasForeignKey(d => d.DocumentId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .HasConstraintName("FK_DocumentChunks_Documents");
        });
    }
}
