using WebApi;
using WebApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.ConfigureSecurity();

builder.Services.ConfigureRateLimiting();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapEndpoints();

app.MapSecurityMiddleware();

app.MapRateLimitingMiddleware();

app.Run();
