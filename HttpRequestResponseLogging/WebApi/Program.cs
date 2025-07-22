using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Endpoints;
using WebApi.Interceptors;
using WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(options =>
{
    // Configure the logging options
    // Log requests and responses - Log all fields
    options.LoggingFields = HttpLoggingFields.All;

    // To Log only specific fields, you can use:
    options.LoggingFields = HttpLoggingFields.RequestMethod |
                            HttpLoggingFields.RequestPath | 
                            HttpLoggingFields.RequestQuery |
                            HttpLoggingFields.RequestHeaders |
                            HttpLoggingFields.ResponseStatusCode |
                            HttpLoggingFields.Duration;

    // Limit the headers that are logged
    options.RequestHeaders.Add("X-Request-ID");
    options.ResponseHeaders.Add("X-Response-Time");
    options.RequestHeaders.Add("Authorization");
    options.ResponseHeaders.Add("User-Agent");
    options.RequestHeaders.Add("X-Forwarded-For");
    options.ResponseHeaders.Add("Content-Type");

    // If the logs are growing too large, you can limit the size of the request body
    options.ResponseBodyLogLimit = 4096; // 4KB
    options.RequestBodyLogLimit = 4096; // 4KB

    if(builder.Environment.IsDevelopment())
    {
        options.LoggingFields|= HttpLoggingFields.RequestBody | 
                                HttpLoggingFields.ResponseBody; // Log request and response bodies in development mode
        // Log the request body in development mode
        options.RequestBodyLogLimit = 1024 * 32; // 32KB
        options.ResponseBodyLogLimit = 1024 * 32; // 32KB
    }

});

// Register the CustomLoggingInterceptor
builder.Services.AddSingleton<IHttpLoggingInterceptor, CustomLoggingInterceptor>();

builder.Services.AddSingleton<IHttpLoggingInterceptor, SensitiveDataReductionInterceptor>();

// HttpClient Logging
builder.Services.AddHttpClient<ITodoClient, TodoClient>(client =>
{
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
}).AddHttpMessageHandler(configure =>
{
    var logger = configure.GetRequiredService<ILoggerFactory>().CreateLogger("json-placeholder-todos");
    return new HttpLoggingHandler(logger);
});

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseApiKeyAuth();

app.UseHttpsRedirection();

app.UseHttpLogging();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseSwaggerUI(opt =>
    opt.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1"));

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});
app.MapUserEndpoints();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
