using ShopHub.Contracts.Events;

namespace ShopHub.WebApi.Services;

public interface IEventPublisher
{
    Task PublishAsync<T>(EventEnvelope<T> envelope, CancellationToken ct=default);

}
