using EmployeesApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController(IEmployeeRepository employeeRepository) : ControllerBase
    {
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetEmployees(Guid id, CancellationToken cancellationToken)
        {
            var result = await employeeRepository.GetEmployeesAsync(id,cancellationToken);
            return Ok(result);
        }

        [HttpGet("companies")]
        public async Task<IActionResult> GetCompanies(CancellationToken cancellationToken)
        {
            var result = await employeeRepository.GetCompanies(cancellationToken);
            return Ok(result);
        }


    }
}
