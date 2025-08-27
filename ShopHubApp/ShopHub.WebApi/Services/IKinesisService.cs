using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using System.Text;

namespace ShopHub.WebApi.Services;

public interface IKinesisService
{
    Task SendAsync(string partitionKey, string data, CancellationToken ct);
}
public class KinesisService : IKinesisService
{
    private readonly AmazonKinesisClient _client;
    private readonly string _streamName;
    public KinesisService(IConfiguration configuration)
    {
        _streamName = configuration["AWS:KinesisStreamName"]?? throw new InvalidOperationException("Kinesis Stream Name is missing");

        _client = new AmazonKinesisClient();
    }
    public async Task SendAsync(string partitionKey, string data, CancellationToken ct)
    {

        // Put Request
        var request = new PutRecordRequest
        {
            StreamName = _streamName,
            PartitionKey = partitionKey,
            Data = new MemoryStream(Encoding.UTF8.GetBytes(data))

        };
       // await _client.PutRecordAsync(request,ct);           
    }
}