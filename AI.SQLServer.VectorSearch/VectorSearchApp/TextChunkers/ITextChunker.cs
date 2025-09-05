namespace VectorSearchApp.TextChunkers;

public interface ITextChunker
{
    IList<string> Split(string text);
}
