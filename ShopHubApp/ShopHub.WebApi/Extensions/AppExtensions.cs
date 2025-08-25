using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using ShopHub.WebApi.Data;

namespace ShopHub.WebApi.Extensions;

public static class AppExtensions 
{
    public static async Task ApplyMigrations(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {

            // To create migrations run the command:
            // dotnet ef migrations add initialCreate --project .\ProductsApp.Infrastructure\ProductsApp.Infrastructure.csproj --startup-project .\EcommerceApp\EcommerceApp.Host.csproj
            // dotnet ef migrations add Initial --startup-project ./ProductService.Host --project ./ProductService.Infrastructure -- --context ProductService.Infrastructure.Database.ApplicationDbContext
            await using (var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>())
            {
                await dbContext.Database.MigrateAsync();
                await DatabaseSeedService.SeedAsync(dbContext);
            }

        }
    }
}
