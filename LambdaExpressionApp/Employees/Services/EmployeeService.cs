using Employees.Data;
using Employees.DTOs;
using Employees.Extensions;
using Employees.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Employees.Services;

public class EmployeeService(AppDbContext dbContext, ILogger<EmployeeService> logger): IEmployeeService
{
    public async Task<IEnumerable<Employee>> GetEmployees(UserLoginModel userModel)
    {
        //var employees = await _dbContext.Employees.ToListAsync();
        IEnumerable<Employee> employees = [];
        Expression<Func<Employee, bool>> isSenior = (x) => x.Age >= 45;
        Expression<Func<Employee, bool>> nameStartswithA = (x) => x.Name.StartsWith("A");
        Expression<Func<Employee, bool>> gmailUser = (x) => x.Email.Contains("@gmail.com");
        if (userModel.Role != "Admin")
        {
            Expression<Func<Employee, bool>> restrictedtoUserCompany = (x) => x.CompanyId == userModel.CompanyId;

            //var combined = isSenior.AndAlso(nameStartswithA);
            var combined = isSenior.AndAlso(restrictedtoUserCompany).AndAlso(nameStartswithA);
            logger.LogInformation("Getting employees for the Role 'User'");
            employees = await dbContext.Employees.Where(combined).ToListAsync();
        }
        else
        {
            var adminCombined = gmailUser.AndAlso(isSenior);
            logger.LogInformation("Getting employees for the Role 'Admin'");
            employees = await dbContext.Employees.Where(adminCombined).ToListAsync();
        }

        return employees;
    }
}
