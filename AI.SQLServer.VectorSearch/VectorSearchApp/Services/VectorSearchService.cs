using DocumentFormat.OpenXml.Office.SpreadSheetML.Y2023.MsForms;
using System.Runtime.CompilerServices;
using VectorSearchApp.Models;

namespace VectorSearchApp.Services;

public class VectorSearchService : IVectorSearchService
{
    public Task<Response> AskQuestionAsync(Question question, bool reformulate = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<Response> AskStreamingAsync(Question question, bool reformulate = true, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ImportDocumentResponse> ImportAsync(Stream stream, string name, string contentType, Guid? documentId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

public interface IVectorSearchService
{
    Task<ImportDocumentResponse> ImportAsync(Stream stream, string name, string contentType, Guid? documentId, CancellationToken cancellationToken=default);

    Task<Response> AskQuestionAsync(Question question, bool reformulate=true, CancellationToken cancellationToken=default);

    IAsyncEnumerable<Response> AskStreamingAsync(Question question, bool reformulate = true, [EnumeratorCancellation] CancellationToken cancellationToken=default);

}
