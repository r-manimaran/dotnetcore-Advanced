
using ShopHub.Contracts.Events;
using System.Text.Json;

namespace ShopHub.WebApi.Services;

public class MultiCloudEventPublisher : IEventPublisher
{
    private readonly IKinesisService _kinesisService;
    private readonly IEventHubService _eventHubService;
    private readonly ILogger<MultiCloudEventPublisher> _logger;

    public MultiCloudEventPublisher(IKinesisService kinesisService, IEventHubService eventHubService, ILogger<MultiCloudEventPublisher> logger)
    {
        _kinesisService = kinesisService;
        _eventHubService = eventHubService;
        _logger = logger;
    }   

    public async Task PublishAsync<T>(EventEnvelope<T> env, CancellationToken ct=default)
    {
        var eventPayload = JsonSerializer.Serialize(env);

        var tasks = new[]
        {
            //PublishToServiceAsync("Kinesis", () => _kinesisService.SendAsync(env.PartitionKey,eventPayload,ct)),
            PublishToServiceAsync("EventHub", () => _eventHubService.SendAsync(env.PartitionKey,eventPayload, ct))
        };

        await Task.WhenAll(tasks);
    }
    
    private async Task PublishToServiceAsync(string serviceName, Func<Task> publishAction)
    {
        try
        {
            await publishAction();
            _logger.LogInformation("Successfully published to {ServiceName}", serviceName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish to {ServiceName}", serviceName);
        }
    }
}
