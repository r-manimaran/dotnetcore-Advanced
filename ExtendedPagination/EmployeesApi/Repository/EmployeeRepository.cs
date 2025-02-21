using EmployeesApi.Data;
using EmployeesApi.Models;
using EmployeesApi.Pagination;
using EmployeesApi.Services;
using Microsoft.EntityFrameworkCore;

namespace EmployeesApi.Repository;

public class EmployeeRepository(AppDbContext dbContext) : IEmployeeRepository
{
    public async Task<IEnumerable<Company>> GetCompanies(CancellationToken cancellationToken)
    {
        var companies = new List<Company>();
        companies = await dbContext.Company
                                .AsNoTracking()
                                .ToListAsync(cancellationToken);
        return companies;
    }

    public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId,
                            EmployeeParameters employeeParameters,
                            CancellationToken cancellationToken = default)
    {

        var employeesQuery = dbContext.Employees
                                .AsNoTracking()
                                .Where(e => e.CompanyId == companyId);
        
        // Apply Age filter only if MinAge and MaxAge are provided
        if(employeeParameters.MinAge > 0)
        {
            
            employeesQuery = employeesQuery.Where(e =>
                    e.Age >= employeeParameters.MinAge);            
        }
        
        // Apply Age filter only if MaxAge are provided
        if(employeeParameters.MaxAge > 0)
        {
            employeesQuery = employeesQuery.Where(e =>
            e.Age <= employeeParameters.MaxAge);
        }

        if (!string.IsNullOrEmpty(employeeParameters.SearchTerm))
        {
           employeesQuery =  employeesQuery.Search(employeeParameters.SearchTerm);
        }
        // Include Company details and order by Name
        employeesQuery = employeesQuery
                                .Include(e => e.Company)
                                //.OrderBy(e => e.Name);
                                .Sort(employeeParameters.OrderBy!);

        var count = await employeesQuery.CountAsync(cancellationToken);

        var employees = await employeesQuery
                        .Skip((employeeParameters.PageNumber - 1) * employeeParameters.PageSize)
                        .Take(employeeParameters.PageSize)
                        .ToListAsync(cancellationToken);

       
        return PagedList<Employee>
            .ToPagedList(employees, count, employeeParameters.PageNumber, employeeParameters.PageSize);
    }

}
