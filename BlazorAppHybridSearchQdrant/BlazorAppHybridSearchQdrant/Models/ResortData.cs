using System.Text.Json.Serialization;

namespace BlazorAppHybridSearchQdrant.Models;

public class ResortData
{
    [JsonPropertyName("hotel_name")]
    public string HotelName { get; set; } = string.Empty;
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
    [JsonPropertyName("location")]
    public string Location { get; set; } = string.Empty;

}
