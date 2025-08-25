namespace ShopHub.WebApi.Services;

public interface IEventPublisher
{
    Task PublishAsync(string eventType, object data);
}
