var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi(opt=>
    opt.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_1);

builder.Services.AddValidation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("/openapi/{documentName}.yaml"); // Yaml based OpenAPI endpoint

    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint(
        "/openapi/v1.yaml", "OpenAPI v1");
    });
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", (int count) =>
{
    var forecast = Enumerable.Range(1, count).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.DisableValidation();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
