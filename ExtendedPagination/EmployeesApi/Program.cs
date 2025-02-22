using EmployeesApi;
using EmployeesApi.Data;
using EmployeesApi.MappingProfile;
using EmployeesApi.Repository;
using EmployeesApi.Services;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig
    .ReadFrom.Configuration(context.Configuration)
    .WriteTo.Console()
    .WriteTo.Seq(builder.Configuration.GetConnectionString("Seq")!)
    .Enrich.WithProperty("ExtendedPaginationApp", context.HostingEnvironment.ApplicationName)
    .Enrich.WithEnvironmentName()
    .Enrich.WithMachineName()
    .Enrich.WithExceptionDetails();
});
builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("EmployeesDb"));

builder.Services.AddScoped<EmployeeSeeder>();

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService,EmployeeService>();

builder.Services.AddAutoMapper(typeof(EmployeeProfile));


// Add Tracing
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource=>resource.AddService("ExtendedPaginationApp"))
    .WithTracing(tracing=>
    {
        tracing
        .AddHttpClientInstrumentation()
        .AddSqlClientInstrumentation()
        .AddAspNetCoreInstrumentation();

        tracing.AddOtlpExporter(options =>
        {
            var endpoint = builder.Configuration.GetConnectionString("Seq")!;
            endpoint = $"{endpoint}/ingest/otlp/v1/traces";
            options.Endpoint = new Uri(endpoint);
            options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
        });

    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint(
        "/openapi/v1.json", "OpenAPI v1");
    });
}

app.UseSerilogRequestLogging(opt =>
{
    opt.EnrichDiagnosticContext = Extensions.HttpRequestEnricher;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.SeedDatabaseAsync();

await app.RunAsync();
