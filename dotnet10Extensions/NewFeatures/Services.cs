using NewFeatures.ExtensionMethods;

namespace NewFeatures;

public class Services
{
    public void ProcessOrders(IEnumerable<string> orders)
    {
        if (orders.IsEmpty())
        {
            Console.WriteLine("No orders to process.");
            return;
        }
        Console.WriteLine($"Processing {orders.ItemCount} orders.");
        // Further processing logic...
        foreach (var order in orders)
        {
            Console.WriteLine(order);
        }
    }
}
