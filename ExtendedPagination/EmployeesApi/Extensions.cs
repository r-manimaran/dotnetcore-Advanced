using EmployeesApi.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeesApi;

public static class Extensions
{
    public static async Task SeedDatabaseAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<EmployeeSeeder>();
        await seeder.SeedAsync();
    }
}
