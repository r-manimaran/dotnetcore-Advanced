using DynamicOptionsDemo.Options;
using Microsoft.Extensions.Options;

namespace DynamicOptionsDemo.Services;

public class OrderService
{
    private readonly TaxOptions _taxOptions;
    public OrderService(IOptionsSnapshot<TaxOptions> taxOptionsSnapshot)
    {
        _taxOptions = taxOptionsSnapshot.Value;
    }

    public double CalculateTax(double amount)
    {
        return amount * _taxOptions.TaxRate;
    }
}
