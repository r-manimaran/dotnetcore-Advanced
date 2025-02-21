using EmployeesApi.Data;
using EmployeesApi.Models;
using EmployeesApi.Utility;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace EmployeesApi;

public static class Extensions
{
    public static async Task SeedDatabaseAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<EmployeeSeeder>();
        await seeder.SeedAsync();
    }

    // Pagination Filter
    public static IQueryable<Employee> FilterEmployees(this IQueryable<Employee> employees, uint minAge, uint maxAge) =>
        employees.Where(e => (e.Age >= minAge && e.Age <= maxAge));

    public static IQueryable<Employee> Search(this IQueryable<Employee> employees, string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            return employees;
        }
        var lowerCaseTerm = searchTerm.Trim().ToLowerInvariant();

        return employees.Where(e=>e.Name.ToLowerInvariant().Contains(lowerCaseTerm));
    }

    public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string orderByQueryString)
    {
        if (string.IsNullOrEmpty(orderByQueryString))
        {
            return employees.OrderBy(e => e.Name);
        }

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<Employee>(orderByQueryString);
        if(string.IsNullOrEmpty(orderQuery))
            return employees.OrderBy(e=>e.Name);

        return employees.OrderBy(orderQuery);
    }

}
