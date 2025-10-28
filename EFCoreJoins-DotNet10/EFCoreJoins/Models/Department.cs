using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreJoins.Models;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

}
