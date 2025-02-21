using EmployeesApi.Repository;
using EmployeesApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController(IEmployeeRepository employeeRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var result = await employeeRepository.GetCompanies();
            return Ok(result);
        }
    }
}
