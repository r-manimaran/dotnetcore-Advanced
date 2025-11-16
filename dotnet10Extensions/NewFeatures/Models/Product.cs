using System;
using System.Collections.Generic;
using System.Text;

namespace NewFeatures.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
