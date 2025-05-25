using NotesApiService.Data;

namespace NotesApiService.Extensions;

public static class AppExtensions
{
   public static async Task  ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
    }
}
