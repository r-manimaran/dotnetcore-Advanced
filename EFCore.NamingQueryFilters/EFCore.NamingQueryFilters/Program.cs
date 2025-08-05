using EFCore.NamingQueryFilters;
using EFCore.NamingQueryFilters.Endpoints;
using EFCore.NamingQueryFilters.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

builder.AddSqlServerClient("sqldb");


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ecommerce")));

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

await app.ApplyMigrations();

app.MapProductsEndpoints();

app.UseHttpsRedirection();

await app.RunAsync();

