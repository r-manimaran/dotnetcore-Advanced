
using VectorSearchApp.Models;

namespace VectorSearchApp.Services;

public interface IDocumentService
{
    Task<IEnumerable<Document>> GetAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<DocumentChunk>> GetChunksAsync(Guid documentId, CancellationToken cancellationToken = default);

    Task<DocumentChunk?> GetChunksEmbeddingAsync(Guid documentId, Guid documentChunkId, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid documentId, CancellationToken cancellationToken= default);

    Task DeleteAsync(IEnumerable<Guid> documentIds, CancellationToken cancellationToken = default);
}
