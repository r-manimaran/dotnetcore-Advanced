//using DocumentFormat.OpenXml.Office.SpreadSheetML.Y2023.MsForms;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using VectorSearchApp.ContentDecoders;
using VectorSearchApp.Data;
using VectorSearchApp.Models;
using VectorSearchApp.Settings;
using ChatResponse = VectorSearchApp.Models.ChatResponse;
using Entities = VectorSearchApp.Data.Entities; 

namespace VectorSearchApp.Services;

public partial class VectorSearchService(IServiceProvider serviceProvider, AppDbContext dbContext, IDocumentService documentService,
    IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator, TokenizerService tokenizerService,IChatService chatService,
    TimeProvider timeProvider, IOptions<AppSettings> appSettingsOptions, ILogger<VectorSearchService> logger) : IVectorSearchService
{
    private readonly AppSettings appSettings = appSettingsOptions.Value;
    public async Task<Response> AskQuestionAsync(Question question, bool reformulate = true, CancellationToken cancellationToken = default)
    {
        var (reformulatedQuestion, embeddingTokenCount, chunks) =await CreateContextAsync(question, reformulate, cancellationToken);

        var (fullanswer, tokenUsage) = await chatService.AskQuestionAsync(question.ConversationId, chunks, reformulatedQuestion.Text!, cancellationToken);

        // Extract the citations from the answer
        var (answer, citations) = ExtractCitations(fullanswer);

        return new(question.Text, reformulatedQuestion.Text!, answer, StreamState.end, new(reformulatedQuestion.TokenUsage, embeddingTokenCount, tokenUsage), citations);
    }

    private static (string answer, IEnumerable<Citation>) ExtractCitations(string fullanswer)
    {
        var citations = new List<Citation>();

        if(string.IsNullOrEmpty(fullanswer))
        {
            return (fullanswer ?? string.Empty, citations);
        }

        var matches = CitationRegEx.Matches(fullanswer);

        foreach(Match match in matches)
        {
            if(match.Success)
            {
                citations.Add(new Citation
                {
                    DocumentId = Guid.Parse(match.Groups["documentId"].Value),
                    ChunkId = Guid.Parse(match.Groups["chunkId"].Value),
                    FileName = match.Groups["fileName"].Value,
                    PageNumber = int.TryParse(match.Groups["pageNumber"].Value, out var pageNumber) && pageNumber > 0? pageNumber  : null,
                    IndexOnPage = int.TryParse(match.Groups["indexOnPage"].Value, out var indexOnPage) ? indexOnPage : 0,
                    Quote = match.Groups["quote"].Value,
                });
            }
        }

        // Remove all content between [ and ]
        var cleanText = RemoveCitationRegEx.Replace(fullanswer, string.Empty).TrimEnd();
        return (cleanText, citations.OrderBy(c => c.FileName).ThenBy(c => c.PageNumber));
    }



    [GeneratedRegex(@"<citation\s+document-id=(?:""|'|)(?<documentId>[^""']*)(?:""|'|)\s+chunk-id=(?:""|'|)(?<chunkId>[^""']*)(?:""|'|)\s+filename=(?:""|'|)(?<filename>[^""']*)(?:""|'|)\s+page-number=(?:""|'|)(?<pageNumber>[^""']*)(?:""|'|)\s+index-on-page=(?:""|'|)(?<indexOnPage>[^""']*)(?:""|'|)>\s*(?<quote>.*?)\s*</citation>", RegexOptions.Singleline)]
    private static partial Regex CitationRegEx { get; }

    [GeneratedRegex(@" [.*?]", RegexOptions.Singleline)]
    private static partial Regex RemoveCitationRegEx { get; }

    private async Task<(ChatResponse reformulatedQuestion, int embeddingTokenCount, IEnumerable<Entities.DocumentChunk> chunks)> CreateContextAsync(Question question, 
        bool reformulate, 
        CancellationToken cancellationToken)
    {
        // Reformulate the question taking into account the context of the chat to perform keyword search and embeddings.
        var reformulatedQuestion = reformulate ? await chatService.CreateQuestionAsync(question.ConversationId, question.Text, cancellationToken) :
                                   new(question.Text);

        var embeddingTokenCount = tokenizerService.CountEmbeddingTokens(reformulatedQuestion.Text!);
        logger.LogDebug("Reformulated question: {ReformulatedQuestion} (tokens: {TokenCount})", reformulatedQuestion.Text, embeddingTokenCount);

        // Perform a vector search to find the most relevant document chunks.
        var questionEmbedding = await embeddingGenerator.GenerateVectorAsync(reformulatedQuestion.Text!, cancellationToken: cancellationToken);

        var chunks = await dbContext.DocumentChunks.Include(c=>c.Document)
            .OrderBy(d=>EF.Functions.VectorDistance("cosine",d.Embedding,questionEmbedding.ToArray()))
            .Take(appSettings.MaxRelevantChunks)
            .ToListAsync(cancellationToken);

        logger.LogDebug("Found {ChunkCount} relevant chunks for the question.", chunks.Count);
        return (reformulatedQuestion, embeddingTokenCount, chunks);

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
