
using System.Text.Json;

namespace ShopHub.WebApi.Services;

public class MultiCloudEventPublisher : IEventPublisher
{
    private readonly IKinesisService _kinesisService;
    private readonly IEventHubService _eventHubService;

    public MultiCloudEventPublisher(IKinesisService kinesisService, IEventHubService eventHubService)
    {
        _kinesisService = kinesisService;
        _eventHubService = eventHubService;
    }
    public async Task PublishAsync(string eventType, object data)
    {
        var eventPayload = JsonSerializer.Serialize(new
        {
            EventType = eventType,
            TimeStamp = DateTime.UtcNow,
            Data = data
        });

        await Task.WhenAll(
            _kinesisService.SendAsync(eventPayload),
            _eventHubService.SendAsync(eventPayload));
    }
}
