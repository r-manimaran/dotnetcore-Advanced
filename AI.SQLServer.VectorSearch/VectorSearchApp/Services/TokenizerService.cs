using Microsoft.Extensions.Options;
using Microsoft.ML.Tokenizers;
using VectorSearchApp.Settings;

namespace VectorSearchApp.Services;

public class TokenizerService(IOptions<AzureOpenAISettings> settingsOptions) : ITokenizerService
{
    private readonly TiktokenTokenizer chatCompletiontokenzier = TiktokenTokenizer.CreateForModel(settingsOptions.Value.ChatCompletion.ModelId);

    private readonly TiktokenTokenizer embeddingTokenzier = TiktokenTokenizer.CreateForModel(settingsOptions.Value.Embedding.ModelId); 
    
    public int CountChatCompletionTokens(string input) => chatCompletiontokenzier.CountTokens(input);
    
    public int CountEmbeddingTokens(string input) => embeddingTokenzier.CountTokens(input);
    
}

public interface ITokenizerService
{
    int CountChatCompletionTokens(string input);

    int CountEmbeddingTokens(string input);
}