using ShopHub.Contracts.Enums;
using ShopHub.Contracts.Models;
using ShopHub.WebApi.Common;

namespace ShopHub.WebApi.Services;

public interface IOrderService
{
    Task<ServiceResult<Order>> GetByIdAsync(string orderId, CancellationToken cancellationToken = default);
    Task<ServiceResult<IEnumerable<Order>>> GetByUserAsync(string userId, CancellationToken cancellationToken = default);
    Task<ServiceResult<Order>> CreateAsync(Order order, CancellationToken cancellationToken = default);
    Task<ServiceResult> UpdateStatusAsync(string orderId, OrderStatus newStatus, CancellationToken cancellationToken = default);
    Task<ServiceResult> UpdatePaymentStatusAsync(string orderId, PaymentStatus newPaymentStatus, CancellationToken cancellationToken = default);
}
