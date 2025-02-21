using EmployeesApi.DTOs;
using EmployeesApi.Models;
using EmployeesApi.Pagination;

namespace EmployeesApi.Services;

public interface IEmployeeService
{
    Task<(IEnumerable<EmployeeDto> employees, Metadata metadata)> GetEmployeesAsync(Guid id, EmployeeParameters parameters, CancellationToken cancellationToken = default);
}
