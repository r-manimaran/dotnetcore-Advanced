namespace EmployeesApi.Models;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public string Designation { get; set; }
    public DateTime DOJ { get; set; }
    public Guid CompanyId { get; set; }
    public Company Company { get; set; }
}


