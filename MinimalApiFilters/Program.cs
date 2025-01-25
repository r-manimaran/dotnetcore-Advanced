using Dapper;
using FluentValidation;
using MinimalApiFilters.Data;
using MinimalApiFilters.Endpoints;
using MinimalApiFilters.Models;
using MinimalApiFilters.Services;
using MinimalApiFilters.Validation;
using System.Data.Entity.Infrastructure;
using IDbConnectionFactory = MinimalApiFilters.Data.IDbConnectionFactory;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDbConnectionFactory>(_ => new SqliteConnectionFactory("Data Source=./customer.db"));

builder.Services.AddSingleton<DatabaseInitializer>();

builder.Services.AddSingleton<ICustomerService, CustomerService>();

builder.Services.AddScoped<IValidator<Customer>, CustomerValidator>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

SqlMapper.AddTypeHandler(new GuidTypeHandler());

SqlMapper.RemoveTypeMap(typeof(Guid));

SqlMapper.RemoveTypeMap(typeof(Guid?));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapCustomerEndpoints();
var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
await databaseInitializer.IntializeAsync();

app.Run();

