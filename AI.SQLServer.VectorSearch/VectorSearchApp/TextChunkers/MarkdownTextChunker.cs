using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel.Text;
using VectorSearchApp.Services;
using VectorSearchApp.Settings;

namespace VectorSearchApp.TextChunkers;

public class MarkdownTextChunker(TokenizerService tokenizerService, 
                    IOptions<AppSettings> appSettingsOptions) : ITextChunker
{
    private readonly AppSettings appSettings = appSettingsOptions.Value;
    public IList<string> Split(string text)
    {
        var lines = TextChunker.SplitMarkDownLines(text, appSettings.MaxTokensPerLine,
                                                   tokenizerService.CountEmbeddingTokens);

        var paragraphs = TextChunker.SplitMarkdownParagraphs(lines, appSettings.MaxTokensPerParagraph, 
            appSettings.OverlapTokens,tokenCounter: tokenizerService.CountChatCompletionTokens);

        return paragraphs;
    }
}
