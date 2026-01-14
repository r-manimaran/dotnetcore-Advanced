using System.Text.Json.Serialization;

namespace SharedLib;

public class Humidity
{
    [JsonPropertyName("percentage")]
    public int Percentage { get; set; }
}
