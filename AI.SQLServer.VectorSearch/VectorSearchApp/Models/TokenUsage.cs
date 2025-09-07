namespace VectorSearchApp.Models;

public record class TokenUsage(int PromptTokens, int CompletionTokens)
{
    public int TokenTokens => PromptTokens + CompletionTokens;
}
