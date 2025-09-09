using DocumentFormat.OpenXml.Spreadsheet;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.Embeddings;
using System.Net.Mime;
using TinyHelpers.Extensions;
using VectorSearchApp.Components;
using VectorSearchApp.ContentDecoders;
using VectorSearchApp.Data;
using VectorSearchApp.Extensions;
using VectorSearchApp.Services;
using VectorSearchApp.Settings;
using VectorSearchApp.TextChunkers;
var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);

// Add services to the container.
var aiSettings = builder.Services.ConfigureAndGet<AzureOpenAISettings>(builder.Configuration, "AzureOpenAI")!;
var appSettings = builder.Services.ConfigureAndGet<AppSettings>(builder.Configuration, nameof(AppSettings))!;

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

builder.Services.AddBlazorBootstrap();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddAzureSql<AppDbContext>(builder.Configuration.GetConnectionString("Default"), options =>
  {
      options.UseVectorSearch();
      options.EnableRetryOnFailure(0);

  }, options =>
  {
      options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
  });

//builder.Services.AddDbContext<AppDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
//    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
//});

builder.Services.AddHybridCache(options =>
{
    options.DefaultEntryOptions = new()
    {
        LocalCacheExpiration = TimeSpan.FromSeconds(10)
    };
});

// Register Services
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IVectorSearchService, VectorSearchService>();
builder.Services.AddScoped<IChatService, ChatService>();

builder.Services.AddSingleton<TokenizerService>();

// Register Content Decoders
builder.Services.AddKeyedSingleton<IContentDecoder, PdfContentDecoder>(MediaTypeNames.Application.Pdf);
builder.Services.AddKeyedSingleton<IContentDecoder, DocxContentDecoder>("application/vnd.openxmlformats-officedocument.wordprocessingml.document");
builder.Services.AddKeyedSingleton<IContentDecoder, TextContentDecoder>(MediaTypeNames.Text.Plain);
builder.Services.AddKeyedSingleton<IContentDecoder, TextContentDecoder>(MediaTypeNames.Text.Markdown);

builder.Services.AddKernel()
    .AddAzureOpenAIChatCompletion(aiSettings.ChatCompletion.Deployment, aiSettings.ChatCompletion.Endpoint, aiSettings.ChatCompletion.ApiKey, modelId: aiSettings.ChatCompletion.ModelId)
    .AddAzureOpenAITextEmbeddingGeneration(aiSettings.Embedding.Deployment, aiSettings.Embedding.Endpoint, aiSettings.Embedding.ApiKey, modelId: aiSettings.Embedding.ModelId);

builder.Services.AddSingleton<IEmbeddingGenerator<string, Embedding<float>>>(serviceProvider =>
{
    var kernel = serviceProvider.GetRequiredService<Kernel>();
    return kernel.GetRequiredService<ITextEmbeddingGenerationService>()
                 .AsEmbeddingGenerator();
});
builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Register Text Chunkers
builder.Services.AddKeyedSingleton<ITextChunker, DefaultTextChunker>(KeyedService.AnyKey);
builder.Services.AddKeyedSingleton<ITextChunker, MarkdownTextChunker>(MediaTypeNames.Text.Markdown);

var app = builder.Build();

await app.ConfigureDatabaseAsync();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    
    app.UseHsts();
}

app.UseAntiforgery();

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Vector Search API v1");
});

app.MapProjectEndpoints();

app.UseHttpsRedirection();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
