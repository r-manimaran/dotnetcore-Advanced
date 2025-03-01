using Employees.DTOs;
using Employees.Services;
using EmployeesApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
        public async Task<IActionResult> GetFilterEmployees(RequestParameter request)
        {
            var response = await _employeeService.GetEmployees(request);
            var header = JsonSerializer.Serialize(response.Metadata);
            Response.Headers.Add("X-Pagination", header);

            return Ok(response);
        }

        [HttpGet("FilterEmployees/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetAorBEmployees(int pageNumber, int pageSize)
        {
            var response = await _employeeService.GetAorBEmployees(pageNumber,pageSize);
            var header = JsonSerializer.Serialize(response.Metadata);
            Response.Headers.Add("X-Pagination", header);
            return Ok(response);
        }
    }
}
