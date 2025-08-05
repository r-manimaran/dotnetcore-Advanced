using EFCore.NamingQueryFilters.Data;
using Microsoft.EntityFrameworkCore;

namespace EFCore.NamingQueryFilters.Extensions;

public static class AppExtensions
{
    public static void AddAppServices(this IServiceCollection services)
    {
        // Register the DbContext with the connection string from configuration
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer("YourConnectionStringHere"));
        // Register other application services if needed
        // e.g., services.AddScoped<IProductService, ProductService>();
    }

    public static async Task ApplyMigrations(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {

            await using (var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>())
            {
                await dbContext.Database.MigrateAsync();
                await DatabaseSeedService.SeedAsync(dbContext);
            }

        }
    }

}
