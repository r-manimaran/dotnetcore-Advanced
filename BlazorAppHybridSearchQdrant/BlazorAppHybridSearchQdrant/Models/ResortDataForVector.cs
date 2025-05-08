using Microsoft.Extensions.VectorData;

namespace BlazorAppHybridSearchQdrant.Models;

public class ResortDataForVector
{
    [VectorStoreRecordKey]
    public Guid Key { get; set; }
    [VectorStoreRecordData]
    public string HotelName { get; set; } = string.Empty;
    [VectorStoreRecordData]
    public string Description { get; set; } = string.Empty;
    [VectorStoreRecordData(IsFullTextIndexed = true)]
    public string Location { get; set; } = string.Empty;
    [VectorStoreRecordVector(4096)]
    public ReadOnlyMemory<float> DescriptionEmbedding { get; set; }
}
