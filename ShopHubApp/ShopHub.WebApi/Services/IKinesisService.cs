using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using System.Text;

namespace ShopHub.WebApi.Services;

public interface IKinesisService
{
    Task SendAsync(string data);
}
public class KinesisService : IKinesisService
{
    private readonly AmazonKinesisClient _client;
    private readonly string _streamName;
    public KinesisService(IConfiguration configuration)
    {
        _streamName = configuration["AWS:KinesisStreamName"];
        _client = new AmazonKinesisClient();
    }
    public async Task SendAsync(string data)
    {
        // Put Request
        var request = new PutRecordRequest
        {
            StreamName = _streamName,
            PartitionKey = Guid.NewGuid().ToString(),
            Data = new MemoryStream(Encoding.UTF8.GetBytes(data))

        };
        await _client.PutRecordAsync(request);
           
    }
}