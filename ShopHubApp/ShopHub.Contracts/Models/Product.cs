using ShopHub.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ShopHub.Contracts.Models;

public class Product
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ProductCategory Category { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string Currency { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreateOnUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdateOnUtc { get; set; }

}
