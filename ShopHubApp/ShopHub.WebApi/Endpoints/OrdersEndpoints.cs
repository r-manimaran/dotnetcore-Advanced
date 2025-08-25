using Carter;
using ShopHub.Contracts.Enums;
using ShopHub.Contracts.Models;
using ShopHub.WebApi.Services;

namespace ShopHub.WebApi.Endpoints;

public class OrdersEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/orders").WithTags("Orders");

        // GET /orders/{id}
        group.MapGet("/{id}", async (string id, IOrderService orderService) =>
        {
            var result = await orderService.GetByIdAsync(id);
            return result.Success? Results.Ok(result.Data) : Results.NotFound(result.ErrorMessage);
        });

        // GET /orders/user/{userId}
        group.MapGet("/user/{userId}", async (string userId, IOrderService orderService) =>
        {
            var result = await orderService.GetByUserAsync(userId);
            return result.Success ? Results.Ok(result.Data) : Results.NotFound(result.ErrorMessage) ;
        });

        // POST Orders
        group.MapPost("/", async (Order order, IOrderService orderService) =>
        {
            var result = await orderService.CreateAsync(order);

            return result.Success ? Results.Created($"/orders/{result.Data?.Id}", result.Data) : Results.BadRequest(result.ErrorMessage);
        });

        // PUT /orders/{id}/status
        group.MapPut("/{id}/status", async (string id, OrderStatus status, IOrderService orderService) =>
        {
            var result = await orderService.UpdateStatusAsync(id, status);
            return result.Success ? Results.NoContent() : Results.NotFound(result.ErrorMessage);
        });

        // PUT /orders/{id}/payment
        group.MapPut("/{id}/status", async (string id, PaymentStatus paymentStatus, IOrderService orderService) =>
        {
            var result = await orderService.UpdatePaymentStatusAsync(id, paymentStatus);
            return result.Success ? Results.NoContent() : Results.NotFound(result.ErrorMessage);
        });
    }
}
