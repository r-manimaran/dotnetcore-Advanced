using BlazorAppHybridSearchQdrant.Models;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using System.Text.Json;

namespace BlazorAppHybridSearchQdrant.Services;

public class DataService : IDataService
{
    private readonly IVectorStoreRecordCollection<Guid, ResortDataForVector> _vectorStoreRecordCollection;
    private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator;
    private readonly IConfiguration _configuration;

    public DataService(
        IVectorStoreRecordCollection<Guid, ResortDataForVector> vectorStoreRecordCollection, 
        IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator,
        IConfiguration configuration)
    {
        _vectorStoreRecordCollection = vectorStoreRecordCollection;
        _embeddingGenerator = embeddingGenerator;
        _configuration = configuration;

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

        var queryVector = await _embeddingGenerator.GenerateVectorAsync(query);

        var collection = (IKeywordHybridSearch<ResortDataForVector>)_vectorStoreRecordCollection;

        HybridSearchOptions<ResortDataForVector> hybridSearchOptions = new()
        {
            VectorProperty = x => x.DescriptionEmbedding,
            AdditionalProperty = x => x.Location,            
        };

        var searchResult = collection.HybridSearchAsync(queryVector, [], 2);

        await foreach (var item in searchResult)
        {
            response.Add($"Resort name:{item.Record.HotelName}, location:{item.Record.Location}," +
                $"description:{item.Record.Description},score:{item.Score}");
        }

        return response;
    }

    public async Task LoadData(string filePath)
    {
        string dataJson = File.ReadAllText(filePath);

        List<ResortData>? resortData = JsonSerializer.Deserialize<List<ResortData>>(dataJson);

        if (resortData != null)
        {
            var tasks = resortData.Select(async resort => new ResortDataForVector
            {
                Key = Guid.NewGuid(),
                Description = resort.Description,
                DescriptionEmbedding = await _embeddingGenerator.GenerateVectorAsync(resort.Description).ConfigureAwait(false),
                HotelName = resort.HotelName,
                Location = resort.Location,
            });

            var data = await Task.WhenAll(tasks).ConfigureAwait(false);
            await _vectorStoreRecordCollection.UpsertAsync(data);
        }
    }
}