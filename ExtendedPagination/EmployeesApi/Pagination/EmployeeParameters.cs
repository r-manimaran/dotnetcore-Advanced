namespace EmployeesApi.Pagination;

public class EmployeeParameters : RequestParameters
{
    //public EmployeeParameters() => OrderBy = "name";
    public uint MinAge { get; set; }
    public uint MaxAge { get; set; }
    public bool ValidAgeRange => MaxAge > MinAge;
    public string? SearchTerm { get; set; }
    public string? OrderBy { get; set; } = "name";
}
