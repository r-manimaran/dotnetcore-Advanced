using Employees.Data;
using Employees.DTOs;
using Employees.Extensions;
using Employees.Models;
using EmployeesApi.DTOs;
using EmployeesApi.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Employees.Services;

public class EmployeeService(AppDbContext dbContext, ILogger<EmployeeService> logger): IEmployeeService
{
    public async Task<PagedList<Employee>> GetEmployees(RequestParameter request)
    {
        //var employees = await _dbContext.Employees.ToListAsync();
        IEnumerable<Employee> employees = [];
        Expression<Func<Employee, bool>> isSenior = (x) => x.Age >= 45;
        Expression<Func<Employee, bool>> nameStartswithA = (x) => x.Name.StartsWith("A");
        Expression<Func<Employee, bool>> gmailUser = (x) => x.Email.Contains("@gmail.com");
        int count = 0;
        if (request.UserLogin.Role != "Admin")
        {
            Expression<Func<Employee, bool>> restrictedtoUserCompany = (x) => x.CompanyId == request.UserLogin.CompanyId;

            //var combined = isSenior.AndAlso(nameStartswithA);
            var combined = isSenior.AndAlso(restrictedtoUserCompany).AndAlso(nameStartswithA);
            logger.LogInformation("Getting employees for the Role 'User'");
            
            var employeesQuery =  dbContext.Employees
                                           .Where(combined);

            count = await employeesQuery.CountAsync();

            employees = await employeesQuery
                .Skip((request.Pagination.PageNumber - 1) * request.Pagination.PageSize)
                .Take(request.Pagination.PageSize)
                .ToListAsync();
                            
        }
        else
        {
            var adminCombined = gmailUser.AndAlso(isSenior);
            logger.LogInformation("Getting employees for the Role 'Admin'");
            
            var employeesQuery = dbContext.Employees
                                          .Where(adminCombined);

            count = await employeesQuery.CountAsync();

            employees = await employeesQuery
                                  .Skip((request.Pagination.PageNumber - 1) * request.Pagination.PageSize)
                                  .Take(request.Pagination.PageSize)
                                  .ToListAsync();
        }

        return PagedList<Employee>.ToPagedList(employees,count,request.Pagination.PageNumber, request.Pagination.PageSize);
    }

    public async Task<PagedList<Employee>> GetAorBEmployees(int pageNumber, int pageSize)
    {
        Expression<Func<Employee, bool>> nameStartswithA = (x) => x.Name.StartsWith("A");
        Expression<Func<Employee, bool>> nameStartswithB = (x) => x.Name.StartsWith("B");

        var combined = nameStartswithA.OrElse(nameStartswithB);

        var employeesQuery = dbContext.Employees
                                      .Where(combined);

        int count = await employeesQuery.CountAsync();

        var employees = await employeesQuery
                              .Skip((pageNumber- 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();

        return PagedList<Employee>.ToPagedList(employees, count,
                                        pageNumber, pageSize);
    }
}
