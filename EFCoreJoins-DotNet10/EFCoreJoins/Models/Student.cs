using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreJoins.Models;

public class Student
{
    public int Id { get; set; }
    public string FirstName { get; set; } =string.Empty;
    public string LastName { get; set; } =string.Empty;
    public string? Email { get; set; } = string.Empty;
    public int?  DepartmentId { get; set; }

    // Navigation Property
    public virtual Department? Department { get; set; }
}
