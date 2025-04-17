using Microsoft.EntityFrameworkCore;
using VectorSearchApp.Components;
using VectorSearchApp.Data;

var builder = WebApplication.CreateBuilder(args);


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

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
