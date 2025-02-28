using Bogus;
using Employees.Data;
using Employees.DTOs;
using Employees.Extensions;
using Employees.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Employees.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public EmployeesController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpPost("")]
        public async Task<IActionResult> GetFilterEmployees(UserLoginModel user)
        {
            //var employees = await _dbContext.Employees.ToListAsync();
            IEnumerable<Employee> employees = [];
            Expression<Func<Employee, bool>> isSenior = (x) => x.Age >= 45;
            Expression<Func<Employee, bool>> nameStartswithA = (x) => x.Name.StartsWith("A");
            Expression<Func<Employee, bool>> gmailUser = (x) => x.Email.Contains("@gmail.com");
            if (user.Role != "Admin")
            {
                Expression<Func<Employee, bool>> restrictedtoUserCompany = (x) => x.CompanyId == user.CompanyId;

                //var combined = isSenior.AndAlso(nameStartswithA);
                var combined = isSenior.AndAlso(restrictedtoUserCompany).AndAlso(nameStartswithA);
                employees = await _dbContext.Employees.Where(combined).ToListAsync();
            }
            else
            {
                var adminCombined = gmailUser.AndAlso(isSenior);
                employees = await _dbContext.Employees.Where(adminCombined).ToListAsync();
            }

            return Ok(employees);
        }
    }
}
