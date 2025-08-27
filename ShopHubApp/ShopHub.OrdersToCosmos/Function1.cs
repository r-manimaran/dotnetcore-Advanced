using System;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ShopHub.OrdersToCosmos;
public class Function1
{
    private readonly ILogger<Function1> _logger;
    private readonly Container _container;

    public Function1(CosmosClient client, ILogger<Function1> logger)
    {
        _logger = logger;
        _container = client.GetContainer("eshop", "order_aggregates");
    }

    [Function(nameof(OrdersToCosmos))]
    public void Run([EventHubTrigger("%EVENT_HUB_NAME%", Connection = "CONSTRHERE")] EventData[] events)
    {


        foreach (EventData @event in events)
        {
            _logger.LogInformation("Event Body: {body}", @event.Body);
            _logger.LogInformation("Event Content-Type: {contentType}", @event.ContentType);
        }
    }
}