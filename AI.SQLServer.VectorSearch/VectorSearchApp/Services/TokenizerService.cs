using Microsoft.Extensions.Options;
using Microsoft.ML.Tokenizers;
using VectorSearchApp.Settings;

namespace VectorSearchApp.Services;

public class TokenizerService(IOptions<AzureOpenAISettings> settingsOptions) : ITokenizerService
{
    private readonly TiktokenTokenizer chatCompletiontokenzier
    public int CountChatCompletionTokens(string input)
    {
        throw new NotImplementedException();
    }

    public int CountEmbeddingTokens(string input)
    {
        throw new NotImplementedException();
    }
}

public interface ITokenizerService
{
    int CountChatCompletionTokens(string input);

    int CountEmbeddingTokens(string input);
}