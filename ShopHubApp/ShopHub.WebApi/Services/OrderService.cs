using Microsoft.EntityFrameworkCore;
using ShopHub.Contracts.Enums;
using ShopHub.Contracts.Models;
using ShopHub.WebApi.Common;
using ShopHub.WebApi.Data;

namespace ShopHub.WebApi.Services;

public class OrderService(AppDbContext dbContext, ILogger<OrderService> logger) : IOrderService
{    
    public async Task<ServiceResult<Order>> CreateAsync(Order order, CancellationToken cancellationToken = default)
    {
        order.Id = Guid.NewGuid().ToString("N");
        order.CreatedOnUtc = DateTime.UtcNow;
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync();

        logger.LogInformation($"Placed the new Order: {order.Id} successfully");

        return ServiceResult<Order>.Ok(order);
    }

    public async Task<ServiceResult<Order>> GetByIdAsync(string orderId, CancellationToken cancellationToken = default)
    {
        var existingOrder = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (existingOrder is null)
        {
            logger.LogWarning($"Order with OrderId:{orderId} does not exists");
            return ServiceResult<Order>.Fail($"Order with OrderId:{orderId} does not exists");
        }
        
        logger.LogInformation($"Retrieved the order with the OrderId: {orderId} successfully");

        return ServiceResult<Order>.Ok(existingOrder);
    }

    public async Task<ServiceResult<IEnumerable<Order>>> GetByUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        var existingOrders = dbContext.Orders.Where(o => o.UserId == userId);

        logger.LogInformation($"Retrieved {existingOrders.Count()} orders for the userId:{userId} successfully");
        
        return ServiceResult<IEnumerable<Order>>.Ok(existingOrders);
    }

    public async Task<ServiceResult> UpdatePaymentStatusAsync(string orderId, PaymentStatus newPaymentStatus, CancellationToken cancellationToken = default)
    {
        var existingOrder = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (existingOrder is null)
        {
            logger.LogWarning($"Order with OrderId:{orderId} does not exists");
            return ServiceResult.Fail($"Order with OrderId:{orderId} does not exists");
        }

        existingOrder.PaymentStatus = newPaymentStatus;
        existingOrder.UpdatedOnUtc = DateTime.UtcNow;
        logger.LogInformation($"Updated payment status: {newPaymentStatus} for the OrderId:{orderId}");
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> UpdateStatusAsync(string orderId, OrderStatus newStatus, CancellationToken cancellationToken = default)
    {
        var existingOrder = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (existingOrder is null)
        {
            logger.LogWarning($"Order with OrderId:{orderId} does not exists");
            return ServiceResult.Fail($"Order with OrderId:{orderId} does not exists");
        }

        existingOrder.Status = newStatus;
        existingOrder.UpdatedOnUtc = DateTime.UtcNow;
        logger.LogInformation($"Upated Status:{newStatus} for the OrderId:{orderId}"); 
        return ServiceResult.Ok();
    }
}
