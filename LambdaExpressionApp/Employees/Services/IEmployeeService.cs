using Employees.DTOs;
using Employees.Models;

namespace Employees.Services;

public interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetEmployees(UserLoginModel userModel);
}
