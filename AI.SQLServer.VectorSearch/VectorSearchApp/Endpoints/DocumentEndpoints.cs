
using VectorSearchApp.Services;

namespace VectorSearchApp.Endpoints;

public class DocumentEndpoints : IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var documentApiGroup = endpoints.MapGroup("/api/documents").WithTags("Documents");

        documentApiGroup.MapGet(string.Empty, async (IDocumentService documentService, CancellationToken cancellationToken) =>
        {

        }).WithSummary("Gets the list of documents");

        documentApiGroup.MapPost(string.Empty, async (IFormFile file, VectorSearchService vectorSearchService, CancellationToken cancellationToken) =>
        {

        })
        .DisableAntiforgery()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Uploads a Document")
        .WithDescription("Uploads a document to a SQL database and save its embedding using the native VECTOR type.The document will be indexed and used to answer questions. Currently, PDF, DOCX, TXT and MD files are supported.");


        documentApiGroup.MapGet("{documentId:guid}/chunks", async (Guid documentId, DocumentService documentService, CancellationToken cancellationToken) =>
        {

        })
        .WithSummary("Gets the list of chunks of a given document")
        .WithDescription("The list does not contain embedding. Use '/api/documents/{documentId}/chunks/{documentChunkId}' to get the embedding for a given chunk.");
        
        documentApiGroup.MapGet("{documentId:guid}/chunks/{documentChunkId:guid}", async (Guid documentId, Guid documentChunkId, DocumentService documentService, CancellationToken cancellationToken) =>
        {

        })
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .WithSummary("Gets the details of a given chunk, includings its embedding");

        documentApiGroup.MapDelete("{documentId:guid}", async (Guid documentId, DocumentService documentService, CancellationToken cancellationToken) =>
        {

        })
        .WithSummary("Delete a document")
        .WithDescription("This endpoint deletes the document and all its chunks");
    }
}
