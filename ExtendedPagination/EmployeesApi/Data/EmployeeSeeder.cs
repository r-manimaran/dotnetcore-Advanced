using Bogus;
using EmployeesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeesApi.Data;

public class EmployeeSeeder
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<EmployeeSeeder> _logger;
    private readonly Faker _faker;
    private const int NUMBER_OF_COMPANIES = 20;
    private const int EMPLOYEES_PER_COMPANY = 50;

    public EmployeeSeeder(AppDbContext dbContext, ILogger<EmployeeSeeder> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        _faker = new Faker();
    }

    public async Task SeedAsync()
    {
        try
        {
            var companies = await SeedCompaniesAsync();
            await SeedEmployeesAsync(companies);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding data");
            throw;
        }
    }

    private async Task<List<Company>> SeedCompaniesAsync()
    {
        if (await _dbContext.Set<Company>().AnyAsync())
        {
            return await _dbContext.Set<Company>().ToListAsync();
        }

        var companies = CreateCompanies();
        await _dbContext.Set<Company>().AddRangeAsync(companies);
        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation($"Successfully seeded {companies.Count} companies");
        return companies;
    }

    private List<Company> CreateCompanies()
    {
        var companyFaker = new Faker<Company>()
            .RuleFor(x => x.Id, f => f.Random.Guid())
            .RuleFor(x => x.Name, f => f.Company.CompanyName());

        return companyFaker.Generate(NUMBER_OF_COMPANIES);
    }

    private async Task SeedEmployeesAsync(List<Company> companies)
    {
        // Get the current Max ID from the database or start from 0 if no employees exist
        var lastEmployeeId = await _dbContext.Set<Employee>().MaxAsync(e => (int?)e.Id) ?? 0;
        
        foreach (var company in companies)
        {
            var employees = CreateEmployees(company.Id, ref lastEmployeeId);
            await _dbContext.Set<Employee>().AddRangeAsync(employees);
            await _dbContext.SaveChangesAsync();
            
            _logger.LogInformation($"Successfully seeded {employees.Count} employees for company {company.Name}");
        }
    }

    private List<Employee> CreateEmployees(Guid companyId, ref int lastEmployeeId)
    {
        var employees = new List<Employee>();

        var employeeFaker = new Faker<Employee>()
            .RuleFor(e => e.Name, f => f.Person.FullName)
            .RuleFor(e => e.Email, f => f.Person.Email)
            .RuleFor(e=>e.Age, f=>f.Random.Int(26,58))
            .RuleFor(e => e.DOJ, f => f.Date.Past())
            .RuleFor(e => e.CompanyId, companyId);

        for(int i=0;i<EMPLOYEES_PER_COMPANY;i++)
        {
            lastEmployeeId++;
            var employee = employeeFaker.Generate();
            employee.Id = lastEmployeeId;
            employees.Add(employee);
        }
        return employees;
    }
}
