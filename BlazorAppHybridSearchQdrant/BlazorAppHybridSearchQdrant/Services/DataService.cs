using BlazorAppHybridSearchQdrant.Models;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;

namespace BlazorAppHybridSearchQdrant.Services;

public class DataService
{
    private readonly IVectorStoreRecordCollection<Guid, ResortDataForVector> _vectorStoreRecordCollection;
    private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator;
    private readonly IConfiguration _configuration;

    public DataService(IVectorStoreRecordCollection<Guid, ResortDataForVector> vectorStoreRecordCollection, IConfiguration configuration)
    {
        _vectorStoreRecordCollection = vectorStoreRecordCollection;
        _configuration = configuration;

        var modelId = configuration["AppSettings:ModelId"] ?? "";
        Uri endpoint = new(configuration["AppSettings:Endpoint"] ?? "");
        _embeddingGenerator = new OllamaEmbeddingGenerator(endpoint, modelId);

        var coExists = _vectorStoreRecordCollection.CollectionExistsAsync().Result;

        if (coExists)
        {
            _vectorStoreRecordCollection.DeleteCollectionAsync().Wait();
        }
        _vectorStoreRecordCollection.CreateCollectionAsync().Wait();
    }

    public async Task<List<string>> GetResortInfo(string query)
    {
        List<string> response = [];

        var queryVector = _embeddingGenerator.GenerateVectorAsync(query).ConfigureAwait(false);

        var collection = (IKeywordHybridSearch<ResortDataForVector>)_vectorStoreRecordCollection;

        return response;
    }
}
