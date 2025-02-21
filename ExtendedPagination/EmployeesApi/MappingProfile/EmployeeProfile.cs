using AutoMapper;
using EmployeesApi.DTOs;
using EmployeesApi.Models;

namespace EmployeesApi.MappingProfile;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeDto>().ForMember(dest => dest.CompanyName,
            opt => opt.MapFrom(src => src.Company.Name));
    }
}
