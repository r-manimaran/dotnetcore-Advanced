using System;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text;

namespace ShopHub.OrdersToCosmos;
public class Function1
{
    private readonly ILogger<Function1> _logger;

    public Function1(ILogger<Function1> logger)
    {
        _logger = logger;
    }

    [Function(nameof(OrdersToCosmos))]
    public void Run([EventHubTrigger("%EVENT_HUB_NAME%", Connection = "EventHubConnection")] EventData[] events)
    {
        _logger.LogInformation("Processing {eventCount} events from EventHub", events.Length);

        foreach (EventData @event in events)
        {
            var eventBody = Encoding.UTF8.GetString(@event.Body.ToArray());
            _logger.LogInformation("Event Body: {body}", eventBody);
            _logger.LogInformation("Event Content-Type: {contentType}", @event.ContentType);
            _logger.LogInformation("Event Properties: {properties}", @event.Properties);
        }
        
        _logger.LogInformation("Completed processing {eventCount} events", events.Length);
    }
}