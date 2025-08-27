using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Text;

namespace ShopHub.WebApi.Services;

public interface IEventHubService
{
    Task SendAsync(string partitionKey, string data, CancellationToken ct);
}

public class EventHubService : IEventHubService, IAsyncDisposable
{
    private readonly EventHubProducerClient _producer;
    private readonly string _eventHubName;

    public EventHubService(IConfiguration configuration)
    {
        var connectionString = configuration["Azure:EventHubConnectionString"];

        _eventHubName = configuration["Azure:EventHubName"] ?? "ShopHub";

        _producer = new EventHubProducerClient(connectionString, _eventHubName);
    }

    public async ValueTask DisposeAsync()
    {
        await _producer.DisposeAsync();
    }

    public async Task SendAsync(string partitionKey, string data, CancellationToken ct)
    {
        using var batch = await _producer.CreateBatchAsync(new CreateBatchOptions
        {
            PartitionKey = partitionKey
        }, ct);

        if (!batch.TryAdd(new EventData(Encoding.UTF8.GetBytes(data))))
            throw new InvalidOperationException("Event too large");
        await _producer.SendAsync(batch,ct);
    }
}
