namespace WebApi.Endpoints;

public static class WeatherForecastEndpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGet("/weatherforecast-minimal", () =>
        {
            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = summaries[Random.Shared.Next(summaries.Length)]
            })
            .ToArray();
        }).WithName("GetWeatherForecast-Minimal")
        .RequireAuthorization()
        .RequireRateLimiting(policyName: "per-user");

    }
}
