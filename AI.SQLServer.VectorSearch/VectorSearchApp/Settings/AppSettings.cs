namespace VectorSearchApp.Settings;

public class AppSettings
{
    public int MaxTokensPerLine { get; set; } = 300;

    public int MaxTokensPerParagraph { get; set; } = 1000;

    public int OverlapTokens { get; set; } = 100;

    public int MaxRelevantChunks { get; set; } = 5;

    public int MaxInputTokens { get; set; } = 16385;

    public int MaxOutputTokens { get; set; } = 800;

    public TimeSpan MessageExpiration { get; set; }

    public int MessageLimit { get; set; } = 20;

    public int EmbeddingBatchSize { get; set; } = 32;

}
