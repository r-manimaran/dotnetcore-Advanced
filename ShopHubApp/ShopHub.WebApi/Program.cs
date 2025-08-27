using Carter;
using Microsoft.Extensions.Options;
using ShopHub.WebApi.Data;
using ShopHub.WebApi.Services;
using Microsoft.EntityFrameworkCore;
using ShopHub.WebApi.Extensions;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Configuration.AddEnvironmentVariables();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? Environment.GetEnvironmentVariable("DBConnectionString");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Database Connection string information was not found in configuration.");
}

builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    });
});

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddSingleton<IKinesisService, KinesisService>();

builder.Services.AddSingleton<IEventHubService, EventHubService>();

builder.Services.AddSingleton<IEventPublisher, MultiCloudEventPublisher>();

builder.Services.AddCarter();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwaggerUI(opt =>
    opt.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1"));

app.UseHttpsRedirection();

app.MapCarter();

await app.ApplyMigrations();

await app.RunAsync();

