namespace DynamicOptionsDemo.Options;

public class TaxOptions
{
    public const string SectionName = nameof(TaxOptions);
    public double TaxRate { get; set; }
}
