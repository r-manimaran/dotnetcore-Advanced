using EFCoreJoins;
using EFCoreJoins.Models;
using Microsoft.EntityFrameworkCore;

using var dbContext = new SchoolDbContext();

await dbContext.Database.EnsureCreatedAsync();
Console.WriteLine("Students and Departments");

var traditionalQuery = await dbContext.Students
    .Include(s => s.Department)
    .Select(s => new
    {
        s.FirstName,
        s.LastName,
        s.Email,
        Department = s.Department != null ? s.Department.Name : "[NONE]"
    }).ToListAsync();

//Display results
foreach (var result in traditionalQuery)
{
    Console.WriteLine($" {result.FirstName} | {result.LastName} | {result.Email} | {result.Department}");
}
Console.WriteLine("-----------------------");
Console.WriteLine(".Net-10 new features: Left join");
var query = dbContext.Students.LeftJoin(dbContext.Departments,
    student => student.DepartmentId,
    department => department.Id,
    (student, department) => new
    {
        student.FirstName,
        student.LastName,
        student.Email,
        Department = department != null ? department.Name : "[NONE]"
    }).ToList();

foreach (var result in query)
{
    Console.WriteLine($" 👤{result.FirstName} | {result.LastName} | 📧 {result.Email} | 🏬 {result.Department}");
}


Console.WriteLine("-----------------------");
Console.WriteLine(".Net-10 new features: Right join");
var rightJoinQuery = dbContext.Students.AsEnumerable().RightJoin(dbContext.Departments.AsEnumerable(),
    student => student.DepartmentId,
    department => department.Id,
    (student, department) => new
    {
        department.Id,
        department.Name,       
        Student = student != null ? student.FirstName : "[NONE]" 
    }).ToList();

foreach (var result in rightJoinQuery)
{
    Console.WriteLine($" 👤{result.Name} | {result.Student}");
}
    