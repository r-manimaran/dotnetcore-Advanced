
using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MimeMapping;
using System.ComponentModel;
using VectorSearchApp.Models;
using VectorSearchApp.Services;

namespace VectorSearchApp.Endpoints;

public class DocumentEndpoints : IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var documentApiGroup = endpoints.MapGroup("/api/documents").WithTags("Documents");

        documentApiGroup.MapGet(string.Empty, async ([FromServices] IDocumentService documentService, CancellationToken cancellationToken) =>
        {
            var documents = await documentService.GetAsync(cancellationToken);
            return TypedResults.Ok(documents);

        }).WithSummary("Gets the list of documents");
    
        documentApiGroup.MapPost(string.Empty, async (IFormFile file, [FromServices]IVectorSearchService vectorSearchService, CancellationToken cancellationToken,
             [Description("The unique identifier of the document. If not provided, a new one will be generated. If you specify an existing documentId, the corresponding document will be overwritten.")] Guid? documentId = null) =>
        {
            using var stream = file.OpenReadStream();

            var response = await vectorSearchService.ImportAsync(stream, file.FileName, MimeUtility.GetMimeMapping(file.FileName),documentId, cancellationToken);

            return TypedResults.Ok(response);
        })
        .DisableAntiforgery()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Uploads a Document")
        .WithDescription("Uploads a document to a SQL database and save its embedding using the native VECTOR type.The document will be indexed and used to answer questions. Currently, PDF, DOCX, TXT and MD files are supported.");
    

       documentApiGroup.MapGet("{documentId:guid}/chunks", async ([FromRoute] Guid documentId,[FromServices]  IDocumentService documentService, CancellationToken cancellationToken) =>
        {
            var documents = await documentService.GetChunksAsync(documentId, cancellationToken);
            return TypedResults.Ok(documents);
        })
        .WithSummary("Gets the list of chunks of a given document")
        .WithDescription("The list does not contain embedding. Use '/api/documents/{documentId}/chunks/{documentChunkId}' to get the embedding for a given chunk.");
        
        documentApiGroup.MapGet("{documentId:guid}/chunks/{documentChunkId:guid}", async Task<Results<Ok<DocumentChunk>,NotFound>> ([FromRoute] Guid documentId, [FromRoute] Guid documentChunkId, [FromServices] IDocumentService documentService, CancellationToken cancellationToken) =>
        {
            var chunk = await documentService.GetChunksEmbeddingAsync(documentId, documentChunkId, cancellationToken);
            if(chunk is null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(chunk);
        })
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .WithSummary("Gets the details of a given chunk, includings its embedding");

        documentApiGroup.MapDelete("{documentId:guid}", async ([FromRoute] Guid documentId, [FromServices]  IDocumentService documentService, CancellationToken cancellationToken) =>
        {
            await documentService.DeleteAsync(documentId, cancellationToken);
            return TypedResults.NoContent();
        })
        .WithSummary("Delete a document")
        .WithDescription("This endpoint deletes the document and all its chunks");
    }
}
