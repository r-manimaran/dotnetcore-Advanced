namespace DynamicOptionsDemo.Models;

public class Order
{
    public int Id { get; set; }
    public double TotalPrice { get; set; }
    public List<OrderItem> Items { get; set; } = new();
}
