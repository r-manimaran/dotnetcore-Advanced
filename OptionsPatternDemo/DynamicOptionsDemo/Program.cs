using DynamicOptionsDemo;
using DynamicOptionsDemo.Endpoints;
using DynamicOptionsDemo.Options;
using DynamicOptionsDemo.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddOpenApi();
// build config to options
builder.Services.Configure<TaxOptions>(builder.Configuration.GetSection(TaxOptions.SectionName));
builder.Services.Configure<LoggingOptions>(builder.Configuration.GetSection(LoggingOptions.SectionName));


// Register the services
builder.Services.AddScoped<OrderService>();

builder.Services.AddSingleton<OrderProcessingService>();

builder.Services.AddHostedService(provider=> provider.GetRequiredService<OrderProcessingService>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint(
        "/openapi/v1.json", "OpenAPI v1");
    });

}
app.UseHttpsRedirection();

app.MapOrdersEndpoints();

app.Run();

