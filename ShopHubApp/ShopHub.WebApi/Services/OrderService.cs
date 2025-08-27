using Microsoft.EntityFrameworkCore;
using ShopHub.Contracts.Enums;
using ShopHub.Contracts.Events;
using ShopHub.Contracts.Models;
using ShopHub.WebApi.Common;
using ShopHub.WebApi.Data;

namespace ShopHub.WebApi.Services;

public class OrderService(AppDbContext dbContext, IEventPublisher publisher, ILogger<OrderService> logger) : IOrderService
{    
    public async Task<ServiceResult<Order>> CreateAsync(Order order, CancellationToken cancellationToken = default)
    {
        order.Id = Guid.NewGuid().ToString("N");
        order.CreatedOnUtc = DateTime.UtcNow;
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync();

        logger.LogInformation($"Stored the Order: {order.Id} in the Database successfully");
        
        // Publish the Order Created Event
        var newOrderEvent = new OrderCreatedV1(
            OrderId: order.Id,
            UserId: order.UserId,
            TotalAmount: order.TotalAmount,
            Currency: order.Currency,
            ProductIds: order.Items.Select(x => x.ProductId).ToArray(),
            Quantities: order.Items.ToDictionary(i => i.ProductId, i => i.Quantity),
            Channel: "Web", // Web or Mobile App
            Region: "us-east",
            createdUtc: DateTime.UtcNow);

        var evt = new EventEnvelope<OrderCreatedV1>(
            MessageId: Guid.NewGuid().ToString(),
            OccurredUtc: DateTime.UtcNow,
            Type: nameof(OrderCreatedV1),
            Version: 1,
            CorrelationId: order.Id,
            PartitionKey: order.Id,
            Payload: newOrderEvent);
        
        await publisher.PublishAsync(evt);
        
        logger.LogInformation($"Created OrderCreatedEvent for the Order:{order.Id} successfully");

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
