using Microsoft.EntityFrameworkCore;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ProductCatalogDb;

namespace ConnectionPoolApi.Extensions;

public static class AppExtensions
{
    public static async Task ApplyMigrations(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            await using (var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>())
            {
                await dbContext.Database.MigrateAsync();
                await DatabaseSeedService.SeedDatabase(dbContext);
            }
        }
    }

    public static void EnableOpenTelemetry(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
       .ConfigureResource(r => r.AddService("efcore.compiledQueries"))
       .WithTracing(tracing =>
            tracing.AddHttpClientInstrumentation()
                   .AddAspNetCoreInstrumentation()
                   .AddEntityFrameworkCoreInstrumentation()
                   )
       .UseOtlpExporter();
    }
}
