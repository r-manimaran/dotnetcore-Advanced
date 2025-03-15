using ConnectionPoolApi;
using ConnectionPoolApi.Extensions;
using ConnectionPoolApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductCatalogDb;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
    options.UseLoggerFactory(LoggerFactory.Create(builder =>
    
    {
         builder.AddConsole()
         .AddDebug()
         .SetMinimumLevel(LogLevel.Information);
    }));
});

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.EnableOpenTelemetry();

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<QueryBenchmark>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwaggerUI(opt =>
    opt.SwaggerEndpoint("/openapi/v1.json", "OpenAPi v1"));

await app.ApplyMigrations();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
