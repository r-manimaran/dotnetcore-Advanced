using ConsoleApp;
using System.Diagnostics;
using System.Reflection;
// Example 1:
Type type = typeof(Greetings);
Console.WriteLine("Class Name:" + type.Name);
object instance = Activator.CreateInstance(type);
MethodInfo method = type.GetMethod("SayGreetings");
method.Invoke(instance, null);



//Example 2:

// BAD
var items = Enumerable.Range(0, 10000)
    .Select(i => new Sample { Name = "Item" + i })
    .ToList();
Console.ReadLine();


foreach(var item in items)
{
    var prop = item.GetType().GetProperty("Name");
    var value = prop.GetValue(item);
}

// BETTER
var type1 = typeof(Sample);
var prop1 = type1.GetProperty("Name");

foreach(var item in items)
{
    var value = prop1.GetValue(item);
}
public class Sample
{
    public string Name { get; set; }
}