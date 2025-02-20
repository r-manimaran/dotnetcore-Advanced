using EmployeesApi.Models;

namespace EmployeesApi.Services;

public interface IEmployeeRepository
{
    Task<IEnumerable<Company>> GetCompanies(CancellationToken cancellationToken=default);
    Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, CancellationToken cancellationToken = default);
}
