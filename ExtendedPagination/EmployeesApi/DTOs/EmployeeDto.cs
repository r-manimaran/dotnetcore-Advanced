namespace EmployeesApi.DTOs;

public class EmployeeDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public DateTime DOJ { get; set; }
    public string Designation { get; set; }
    public string CompanyName { get; set; }
}
