using Bogus;
using Employees.Data;
using Employees.DTOs;
using Employees.Extensions;
using Employees.Models;
using Employees.Services;
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
        
        private readonly IEmployeeService _employeeService;
        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }


        [HttpPost("")]
        public async Task<IActionResult> GetFilterEmployees(UserLoginModel user)
        {
            var response = await _employeeService.GetEmployees(user);
            return Ok(response);
        }
    }
}
