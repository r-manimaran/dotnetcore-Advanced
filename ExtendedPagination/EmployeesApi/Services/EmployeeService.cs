using AutoMapper;
using EmployeesApi.DTOs;
using EmployeesApi.Models;
using EmployeesApi.Pagination;
using EmployeesApi.Repository;

namespace EmployeesApi.Services;

public class EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper) : IEmployeeService
{
    public async Task<(IEnumerable<EmployeeDto> employees, Metadata metadata)> GetEmployeesAsync(Guid id, 
                        EmployeeParameters parameters, 
                        CancellationToken cancellationToken = default)
    {
        if(!parameters.ValidAgeRange)
        {
            // handle the error flow
        }

        var employeesWithMetadata = await employeeRepository.GetEmployeesAsync(id, parameters, cancellationToken);
        var employeesDto = mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetadata);
        return (employees: employeesDto, metadata: employeesWithMetadata.Metadata);
    }
}
