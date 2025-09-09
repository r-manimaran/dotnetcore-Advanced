namespace VectorSearchApp.Extensions;

public static class StreamExtensions
{
    public static async Task<MemoryStream> GetMemoryStreamAsync(this Stream stream)
    { 
        using var bufferedStream = new BufferedStream(stream);

        var ms = new MemoryStream();
        await bufferedStream.CopyToAsync(ms);

        ms.Position = 0;
        return ms;
    }
}
