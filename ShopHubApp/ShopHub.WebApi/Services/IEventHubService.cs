using Azure.Messaging.EventHubs.Producer;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace ShopHub.WebApi.Services;

public interface IEventHubService
{
    Task SendAsync(string data);
}

public class EventHubService : IEventHubService
{
    private readonly EventHubProducerClient _producer;
    private readonly string _eventHubName;
    public EventHubService(IConfiguration configuration)
    {
        var connectionString = configuration["Azure:EventHubConnectionString"];
        _eventHubName = configuration["Azure:EventHubName"];
        _producer = new EventHubProducerClient(connectionString, _eventHubName);

    }
    public Task SendAsync(string data)
    {
        throw new NotImplementedException();
    }
}
