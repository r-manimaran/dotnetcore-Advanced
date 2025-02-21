using EmployeesApi.Pagination;
using EmployeesApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EmployeesApi.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    public class EmployeesController(IEmployeeService employeeService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetEmployees(Guid companyId,
                            [FromQuery] EmployeeParameters employeeParameters,
                            CancellationToken cancellationToken)
        {
            var pageResult = await employeeService
                            .GetEmployeesAsync(companyId, 
                                               employeeParameters,
                                               cancellationToken);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pageResult.metadata));

            return Ok(pageResult.employees);
        }

    }
}
