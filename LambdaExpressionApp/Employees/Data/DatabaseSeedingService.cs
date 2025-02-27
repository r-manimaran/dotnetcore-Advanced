using Bogus;
using Employees.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Employees.Data;

public static class DatabaseSeedingService
{
    public static async Task SeedAsync(AppDbContext dbContext)
    {
        if (await dbContext.Employees.AnyAsync())
        {
            return;
        }
        var companyFaker = new Faker<Company>()
                .RuleFor(c => c.Name, f => f.Company.CompanyName());

        var employeesFaker =  new Faker<Employee>()
                .RuleFor(x => x.Name, f => f.Person.FullName)
                .RuleFor(x => x.Email, f => f.Person.Email)
                .RuleFor(x => x.Age, f => f.Random.Int(18, 60))
                .RuleFor(x => x.Company, f => companyFaker.Generate());

        var companies = companyFaker.Generate(5);
        await dbContext.Companies.AddRangeAsync(companies);
        foreach(var company in companies)
        {
            var employees = employeesFaker.RuleFor(p => p.Company, company).Generate(50);
            await dbContext.Employees.AddRangeAsync(employees);
        }      

        await dbContext.SaveChangesAsync();
    }

    
    
}
