using EmployeesApi.Data;
using EmployeesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeesApi.Services;

public class EmployeeRepository(AppDbContext dbContext) : IEmployeeRepository
{
    public async Task<IEnumerable<Company>> GetCompanies(CancellationToken cancellationToken)
    {
        var companies = new List<Company>();
        companies=await dbContext.Company
                                .AsNoTracking()
                                .ToListAsync(cancellationToken);
        return companies;
    }

    public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, CancellationToken cancellationToken = default)
    {
        var employees = new List<Employee>();
        employees = await dbContext.Employees
                                   .AsNoTracking()
                                   .Where(e=>e.CompanyId == companyId)
                                   .Include(c=>c.Company)
                                   .ToListAsync(cancellationToken);
        return employees;
    }

}
