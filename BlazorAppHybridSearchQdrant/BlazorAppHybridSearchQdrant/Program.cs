using BlazorAppHybridSearchQdrant.Components;
using BlazorAppHybridSearchQdrant.Models;
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager config = builder.Configuration;

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var kernelBuilder = builder.Services.AddKernel();
kernelBuilder.AddQdrantVectorStoreRecordCollection<Guid, ResortDataForVector>(config["AppSettings:QdrantCollectionName"] ?? "", "localhost");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
