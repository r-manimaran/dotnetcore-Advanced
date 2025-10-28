using EFCoreJoins.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreJoins;

public class SchoolDbContext :DbContext
{
    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<Department> Departments { get; set; } = null!;
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("SchoolDb");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Department>().HasData(
        new Department { Id = 1, Name = "Computer Science", Description="Computer Science Department" },
        new Department { Id = 2, Name = "Mathematics", Description="Mathematics Department" },
        new Department { Id = 3, Name = "Physics", Description="Physics Department"  }
      );

        modelBuilder.Entity<Student>().HasData(
            new Student { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", DepartmentId = 1 },
            new Student { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", DepartmentId = 2 },
            new Student { Id = 3, FirstName = "Alice", LastName = "Johnson", Email = "alice.johnson@example.com", DepartmentId = 3 },
            new Student { Id = 4, FirstName = "Bob", LastName = "Williams", Email = "bob.williams@example.com", DepartmentId = 1 },
            new Student { Id = 5, FirstName = "Emily", LastName = "Brown", Email = "emily.brown@example.com", DepartmentId = null },
            new Student { Id = 6, FirstName = "Michael", LastName = "Davis", Email = "michael.davis@example.com", DepartmentId = 3 },
            new Student { Id = 7, FirstName = "Sophia", LastName = "Miller", Email = "sophia.miller@example.com", DepartmentId = 1 },
            new Student { Id = 8, FirstName = "Oliver", LastName = "Wilson", Email = "oliver.wilson@example.com", DepartmentId = 2 },
            new Student { Id = 9, FirstName = "Emma", LastName = "Moore", Email = "emma.moore@example.com", DepartmentId = null },
            new Student { Id = 10, FirstName = "Daniel", LastName = "Taylor", Email = "daniel.taylor@example.com", DepartmentId = null }
            );

    }
}
