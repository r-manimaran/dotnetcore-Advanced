using Microsoft.EntityFrameworkCore;
using VectorSearchApp.Data;

namespace VectorSearchApp.Extensions;

public static class AppExtensions
{
    public static async Task ConfigureDatabaseAsync(this IApplicationBuilder applicationBuilder)
    {
        await using var scope = applicationBuilder.ApplicationServices.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}
