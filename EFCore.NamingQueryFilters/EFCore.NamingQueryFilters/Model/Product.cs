namespace EFCore.NamingQueryFilters.Model;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;  
    public decimal Price { get; set; } = 0.0m;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; }

}
