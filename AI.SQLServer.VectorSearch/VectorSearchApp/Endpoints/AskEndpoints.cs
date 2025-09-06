
using System.ComponentModel;
using VectorSearchApp.Models;
using VectorSearchApp.Services;
using FluentValidation;
using MinimalHelpers.FluentValidation;

namespace VectorSearchApp.Endpoints;

public class AskEndpoints : IEndpointRouteHandlerBuilder
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/ask", async (Question question, VectorSearchService vectorSearchService, CancellationToken cancellationToken,
            [Description("If true, the question will be reformulated taking into account the context of the chat identified by the conversationId")]bool reformulate =true) =>
        {
            // This is a placeholder for the actual implementation of the ask endpoint.

            var response = await vectorSearchService.AskQuestionAsync(question, reformulate, cancellationToken);

            return TypedResults.Ok(response);
        })
        .WithValidation<Question>()
        .WithName("Ask")
        .WithSummary("Asks a question")
        .WithDescription("The question will be reformulated taking into account the context of the chat identified by the given ConversationId")
        .WithTags("Ask");


        endpoints.MapPost("/api/ask/streaming", async (Question question, VectorSearchService vectorSearchService, CancellationToken cancellationToken,
            [Description("If true, the question will be reformulated taking into account the context of the chat identified by the conversationId")]bool reformulate = true) =>
        {
            async IAsyncEnumerable<Response> Stream()
            {
                var responseStream = vectorSearchService.AskStreamingAsync(question, reformulate, cancellationToken);

                await foreach (var delta in responseStream)
                {
                    yield return delta;
                }

            }
            return Stream();
        })
        .WithValidation<Question>()
        .WithName("AskStreaming")
        .WithSummary("Asks a question in streaming mode")
        .WithDescription("The question will be reformulated taking into account the context of the chat identified by the given ConversationId")
        .WithTags("Ask");

    }
}
