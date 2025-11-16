using NewFeatures.ExtensionMethods;
using NewFeatures.ExtensionMethods.New;
using NewFeatures.Models;

Console.WriteLine("Hello, World! This project is using C# 14.0 features.");
string input = "Hello world!";

Console.WriteLine(input.IsNullOrEmpty());

var product = Product.CreateDefault();
if(Product.IsValidPrice(999.99m))
{
    product.Price = 999.99m;
}
