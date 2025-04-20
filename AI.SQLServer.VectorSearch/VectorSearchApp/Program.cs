using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VectorSearchApp.Components;
using VectorSearchApp.Data;
using VectorSearchApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer(); // Add this line
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Vector Search API",
        Version = "v1",
        Description = "API for vector-based document search"
    });
});

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddAzureSql<AppDbContext>(builder.Configuration.GetConnectionString("Default"), options =>
  {
      options.UseVectorSearch();

  }, options =>
  {
      options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
  });

builder.Services.AddHybridCache(options =>
{
    options.DefaultEntryOptions = new()
    {
        LocalCacheExpiration = TimeSpan.FromSeconds(10)
    };
});

builder.Services.AddScoped<IDocumentService, DocumentService>();

builder.Services.AddScoped<IVectorSearchService, VectorSearchService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    
    app.UseHsts();
}

//app.UseSwaggerUI(options =>
//    options.SwaggerEndpoint("/openapi/v1.json", "OpenApi v1"));

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Vector Search API v1");
});

app.MapEndpoints();

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
