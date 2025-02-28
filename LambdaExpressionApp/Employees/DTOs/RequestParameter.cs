using Employees.DTOs;

namespace EmployeesApi.DTOs;

public class RequestParameter
{
    public Pagination Pagination { get; set; }
    public UserLoginModel UserLogin { get; set; }
}

public class Pagination
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
