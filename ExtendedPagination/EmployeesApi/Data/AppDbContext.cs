using EmployeesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeesApi.Data;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
    {
        
    }
    public DbSet<Company> Company { get; set; }
    public DbSet<Employee> Employees { get; set; } 
}
