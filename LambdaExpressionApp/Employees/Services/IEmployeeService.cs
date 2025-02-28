using Employees.DTOs;
using Employees.Models;
using EmployeesApi.DTOs;

namespace Employees.Services;

public interface IEmployeeService
{
    Task<PagedList<Employee>> GetEmployees(RequestParameter request);
}
