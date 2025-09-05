using Microsoft.EntityFrameworkCore;
using VectorSearchApp.Data;
using VectorSearchApp.Endpoints;

namespace VectorSearchApp.Extensions;

public static class AppExtensions
{
    public static async Task ConfigureDatabaseAsync(this IApplicationBuilder applicationBuilder)
    {
        await using var scope = applicationBuilder.ApplicationServices.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await dbContext.Database.MigrateAsync();
    }

    public static T? ConfigureAndGet<T>(this IServiceCollection services, IConfiguration configuration, string sectionName) where T : class
    {
        var section = configuration.GetSection(sectionName);
        var settings = section.Get<T>();
        services.Configure<T>(section);

        return settings;
    }
    public static void MapProjectEndpoints(this IEndpointRouteBuilder endpoints)
    {
        DocumentEndpoints.MapEndpoints(endpoints);
    }
}
