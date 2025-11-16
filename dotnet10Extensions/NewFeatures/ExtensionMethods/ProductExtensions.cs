using NewFeatures.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewFeatures.ExtensionMethods;

public static class ProductExtensions
{
    extension(Product)
    {
        public static Product CreateDefault() =>
            new Product
            {
                Id = 1,
                Name = "Default Product",
                Price = 0.0m
            };
        public static bool IsValidPrice(decimal price) => price >= 0.0m && price <= 100000;

        public static string DefaultCategory => "General";
    }
}
