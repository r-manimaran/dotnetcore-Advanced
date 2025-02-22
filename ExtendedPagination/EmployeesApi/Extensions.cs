using EmployeesApi.Data;
using EmployeesApi.Models;
using EmployeesApi.Utility;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Linq.Dynamic.Core;
using System.Security.Claims;

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

    //Enrichers
    public static void HttpRequestEnricher(IDiagnosticContext context, HttpContext httpContext)
    {
        var httpContextInfo = new HttpContextInfo
        {
            Protocol = httpContext.Request.Protocol,
            Scheme = httpContext.Request.Scheme,
            IPAddress = httpContext.Connection.RemoteIpAddress.ToString(),
            HostName = httpContext.Request.Host.ToString(),
            User = GetUserInfo(httpContext.User)
        };
        context.Set("HttpContext",httpContextInfo,true);
    }

    private static string GetUserInfo(ClaimsPrincipal user)
    {
        if (user.Identity != null && user.Identity.IsAuthenticated)
        {
            return user.Identity.Name;
        }
        return Environment.UserName;
    }

}
