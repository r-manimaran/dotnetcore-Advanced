using ShopHub.Contracts.Enums;

namespace ShopHub.Contracts.Models;

public class Order
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public List<OrderItem> Items { get; set; }
    public decimal TotalAmount => Items.Sum(x => x.UnitPrice * x.Quantity);
    public string Currency { get; set; } = "USD";
    public OrderStatus Status { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? UpdatedOnUtc { get;set; }

}
