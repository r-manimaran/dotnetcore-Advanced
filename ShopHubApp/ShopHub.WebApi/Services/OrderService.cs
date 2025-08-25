using ShopHub.Contracts.Enums;
using ShopHub.Contracts.Models;
using ShopHub.WebApi.Common;
using ShopHub.WebApi.Data;

namespace ShopHub.WebApi.Services;

public class OrderService(AppDbContext dbContext, ILogger<OrderService> logger) : IOrderService
{    
    public Task<ServiceResult<Order>> CreateAsync(Order order, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult<Order>> GetByIdAsync(string orderId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult<IEnumerable<Order>>> GetByUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult> UpdatePaymentStatusAsync(string orderId, PaymentStatus newPaymentStatus, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResult> UpdateStatusAsync(string orderId, OrderStatus newStatus, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
