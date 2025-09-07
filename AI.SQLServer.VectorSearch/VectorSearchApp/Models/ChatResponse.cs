namespace VectorSearchApp.Models;

public record class ChatResponse(string? Text, TokenUsage? TokenUsage=null);
