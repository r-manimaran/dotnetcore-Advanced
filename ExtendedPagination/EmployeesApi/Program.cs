using EmployeesApi;
using EmployeesApi.Data;
using EmployeesApi.MappingProfile;
using EmployeesApi.Repository;
using EmployeesApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("EmployeesDb"));

builder.Services.AddScoped<EmployeeSeeder>();

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService,EmployeeService>();

builder.Services.AddAutoMapper(typeof(EmployeeProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint(
        "/openapi/v1.json", "OpenAPI v1");
    });
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.SeedDatabaseAsync();

await app.RunAsync();
