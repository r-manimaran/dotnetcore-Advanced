namespace VectorSearchApp.Models;

public record class Response(string? OriginalQuestion, string? ReformuatedQuestion)
{
    public Response():this(null,null)
    {
        
    }
}
