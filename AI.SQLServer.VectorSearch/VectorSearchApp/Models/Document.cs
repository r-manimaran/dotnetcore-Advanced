namespace VectorSearchApp.Models;

public record class Document(Guid Id, string Name, DateTimeOffset creationDate, int ChunkCount);
