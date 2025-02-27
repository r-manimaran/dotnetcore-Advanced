using System.Reflection;

namespace Employees.Models;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; } 
    public DateTime DOJ { get; set; }
    public Company Company { get; set; }
    public int CompanyId { get; set; }
}
