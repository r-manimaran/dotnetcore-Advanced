using System.Runtime.Serialization;

namespace VectorSearchApp.Models;

public record class Response(string? OriginalQuestion, string? ReformuatedQuestion, string? Answer, StreamState? streamState=null,TokenUsageResponse? TokenUsage=null, IEnumerable<Citation>? Citations=null)
{
    public Response(string? token, StreamState streamState, TokenUsageResponse? tokenUsageResponse=null, IEnumerable<Citation>? citations=null)
        :this(null,null, token, streamState, tokenUsageResponse, citations)
    {
        
    }
}
