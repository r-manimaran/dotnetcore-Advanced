using DynamicOptionsDemo.Models;
using DynamicOptionsDemo.Options;
using DynamicOptionsDemo.Services;
using Microsoft.Extensions.Options;

namespace DynamicOptionsDemo.Endpoints;

public static class OrdersEndpoints
{
    public static void MapOrdersEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/placeOrder", async (Order newOrder, OrderService orderService,IOptionsSnapshot<TaxOptions> taxOptions, ILogger<Program> logger)  =>
        {
            logger.LogInformation("Placing order: {order}", newOrder);
            logger.LogInformation("Calculating total price");
            var tax = orderService.CalculateTax(newOrder.TotalPrice);
            var total = newOrder.TotalPrice + tax;
            return Results.Ok(new
            {
                OrderPrice = total,
                TaxPercentage = taxOptions.Value.TaxRate,
                Tax = tax,
                Total = total
            });
        });
    }
}
