using EmployeesApi.Models;
using EmployeesApi.Pagination;

namespace EmployeesApi.Repository;

public interface IEmployeeRepository
{
    Task<IEnumerable<Company>> GetCompanies(CancellationToken cancellationToken=default);
    Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId,
                            EmployeeParameters employeeParameters,
                            CancellationToken cancellationToken = default);
}
