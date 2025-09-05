using DocumentFormat.OpenXml.Office.SpreadSheetML.Y2023.MsForms;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using VectorSearchApp.ContentDecoders;
using VectorSearchApp.Data;
using VectorSearchApp.Models;
using VectorSearchApp.Settings;
using Entities = VectorSearchApp.Data.Entities; 

namespace VectorSearchApp.Services;

public class VectorSearchService(IServiceProvider serviceProvider, AppDbContext dbContext, IDocumentService documentService,
    IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator, TokenizerService tokenizerService,
    TimeProvider timeProvider, IOptions<AppSettings> appSettingsOptions, ILogger<VectorSearchService> logger) : IVectorSearchService
{
    private readonly AppSettings appSettings = appSettingsOptions.Value;
    public Task<Response> AskQuestionAsync(Question question, bool reformulate = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<Response> AskStreamingAsync(Question question, bool reformulate = true, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<ImportDocumentResponse> ImportAsync(Stream stream, string name, string contentType, Guid? documentId, CancellationToken cancellationToken = default)
    {
        // Extract the contents of the file.
        var decoder = serviceProvider.GetKeyedService<IContentDecoder>(contentType) ?? throw new NotSupportedException($"Content type '{contentType}' is not supported");
        
        var chunks = await decoder.DecodeAsync(stream, contentType, cancellationToken);

        var chunkContents = chunks.Select(p=>p.Content).ToList();

        var tokenCount = tokenizerService.CountEmbeddingTokens(string.Join(" ", chunkContents));

        var strategy = dbContext.Database.CreateExecutionStrategy();

      //  var document = await strategy.ExecuteAsync((object?)null, async(context, state,ct) =>
       // {
            await dbContext.Database.BeginTransactionAsync(cancellationToken);
            if(documentId.HasValue)
            {
                // If the user is importing a document that is already present in the database, delete the previous one.
                await documentService.DeleteAsync(documentId.Value, cancellationToken);
            }

            var document = new Entities.Document { Id = documentId.GetValueOrDefault(), Name = name, CreationDate = timeProvider.GetUtcNow() };
            dbContext.Documents.Add(document);

            // Process each paragraph in batches
            var embeddings = new List<Embedding<float>>();
            foreach (var batch in chunkContents.Chunk(appSettings.EmbeddingBatchSize))
            {
                logger.LogDebug("Processing batch of {Count} chunks for embedding generation...", batch.Length);

                // Generate Emeddings for each batch
                var batchEmbeddings = await embeddingGenerator.GenerateAsync(batch, cancellationToken: cancellationToken);
                embeddings.AddRange(batchEmbeddings);
            }

            // save each document chunks and its embeddings in the database
            foreach(var (index,embedding) in embeddings.Index())
            {
                var chunk = chunks.ElementAt(index);
                logger.LogDebug("Storing a chunk of {TokenCount} tokens.", tokenizerService.CountChatCompletionTokens(chunk.Content));

                var documentChunk = new Entities.DocumentChunk
                {
                    Document = document,
                    Index = index,
                    PageNumber = chunk.PageNumber,
                    IndexOnPage = chunk.IndexOnPage,
                    Content = chunk.Content,
                    Embedding = embedding.Vector.ToArray(),
                };

                dbContext.DocumentChunks.Add(documentChunk);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            await dbContext.Database.CommitTransactionAsync(cancellationToken);

          //  return document;
        //},null, cancellationToken);
        return new ImportDocumentResponse(document.Id,tokenCount);
    }
}

public interface IVectorSearchService
{
    Task<ImportDocumentResponse> ImportAsync(Stream stream, string name, string contentType, Guid? documentId, CancellationToken cancellationToken=default);

    Task<Response> AskQuestionAsync(Question question, bool reformulate=true, CancellationToken cancellationToken=default);

    IAsyncEnumerable<Response> AskStreamingAsync(Question question, bool reformulate = true, [EnumeratorCancellation] CancellationToken cancellationToken=default);

}
