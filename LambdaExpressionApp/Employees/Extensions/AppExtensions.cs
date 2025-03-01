using Employees.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Employees.Extensions;

public static partial class AppExtensions
{
    public static async Task ApplyMigrations(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            await using (var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>())
            {
                await dbContext.Database.MigrateAsync();
                await DatabaseSeedingService.SeedAsync(dbContext);
            }

        }
    }

}
