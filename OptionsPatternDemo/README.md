# Options Pattern Demo - IOptionsSnapshot vs IOptionsMonitor

This project demonstrates the differences between `IOptionsSnapshot<T>` and `IOptionsMonitor<T>` in .NET Core, showcasing how each handles configuration changes and their appropriate use cases.

## 🎯 Project Overview

The demo implements an order processing system that uses both options patterns to handle:
- **Tax calculations** using `IOptionsSnapshot<TaxOptions>`
- **Dynamic logging levels** using `IOptionsMonitor<LoggingOptions>`

## 📋 Key Concepts

### IOptionsSnapshot<T>
- **Scoped lifetime**: New instance per HTTP request
- **Configuration reload**: Values updated per request, not during request
- **Use case**: Web applications where config changes should apply to new requests
- **Thread-safe**: Yes, but values are cached per request

### IOptionsMonitor<T>
- **Singleton lifetime**: Same instance throughout application lifetime  
- **Real-time updates**: Immediately reflects configuration changes
- **Change notifications**: Supports `OnChange` callbacks
- **Use case**: Background services, real-time configuration updates
- **Thread-safe**: Yes, with immediate updates

## 🏗️ Project Structure

```
DynamicOptionsDemo/
├── Options/
│   ├── TaxOptions.cs          # Tax rate configuration
│   └── LoggingOptions.cs      # Logging level configuration
├── Services/
│   └── OrderService.cs        # Uses IOptionsSnapshot for tax calculations
├── Endpoints/
│   └── OrdersEndpoints.cs     # API endpoints for order processing
├── Models/
│   ├── Order.cs              # Order model
│   └── OrderItem.cs          # Order item model
├── OrderProcessingService.cs  # Background service using IOptionsMonitor
└── Program.cs                # Application setup and DI configuration
```

## ⚙️ Configuration

### appsettings.json
```json
{
  "TaxOptions": {
    "TaxRate": 0.06
  },
  "LoggingOptions": {
    "LogLevel": "Information"
  }
}
```

## 🚀 Getting Started

### Prerequisites
- .NET 9.0 SDK
- Visual Studio 2022 or VS Code

### Running the Application

1. **Clone and navigate to project**
   ```bash
   cd OptionsPatternDemo/DynamicOptionsDemo
   ```

2. **Run the application**
   ```bash
   dotnet run
   ```

3. **Access Swagger UI**
   - Navigate to `https://localhost:7xxx/swagger`
   - Use the `/placeOrder` endpoint to test tax calculations

## 🧪 Testing the Options Patterns

### Testing IOptionsSnapshot (Tax Calculations)

1. **Place an order** via Swagger or HTTP client:
   ```json
   POST /placeOrder
   {
     "id": 1,
     "totalPrice": 100.0,
     "items": []
   }
   ```

2. **Modify tax rate** in `appsettings.json`:
   ```json
   "TaxOptions": {
     "TaxRate": 0.08
   }
   ```

3. **Make new request** - New tax rate applies to subsequent requests only

### Testing IOptionsMonitor (Logging Levels)

1. **Observe background service logs** in console
2. **Change logging level** in `appsettings.json`:
   ```json
   "LoggingOptions": {
     "LogLevel": "Warning"
   }
   ```
3. **Watch real-time change** - Logging immediately updates without restart

## 📊 Comparison Table

| Feature | IOptionsSnapshot | IOptionsMonitor |IOptions<T> |
|---------|------------------|-----------------|------------|
| **Lifetime** | Scoped (per request) | Singleton |Singleton|
| **Update Timing** | Next request | Immediate | No |
| **Change Notifications** | ❌ No | ✅ Yes (`OnChange`) |No|
| **Performance** | Cached per request | Always current | No change |
| **Best For** | Web requests | Background services |Static config|
| **Memory Usage** | Higher (multiple instances) | Lower (single instance) | |

## 🔧 Implementation Details

### OrderService (IOptionsSnapshot)
```csharp
public class OrderService
{
    private readonly TaxOptions _taxOptions;
    
    public OrderService(IOptionsSnapshot<TaxOptions> taxOptionsSnapshot)
    {
        _taxOptions = taxOptionsSnapshot.Value; // Cached for request scope
    }
}
```

### OrderProcessingService (IOptionsMonitor)
```csharp
public class OrderProcessingService : BackgroundService
{
    private LoggingOptions loggingOptions;
    
    public OrderProcessingService(IOptionsMonitor<LoggingOptions> optionsMonitor)
    {
        loggingOptions = optionsMonitor.CurrentValue;
        
        optionsMonitor.OnChange((options) => {
            loggingOptions = options; // Real-time updates
        });
    }
}
```

## 🎓 Learning Outcomes

After running this demo, you'll understand:

1. **When to use IOptionsSnapshot**: Web applications where configuration changes should apply to new requests
2. **When to use IOptionsMonitor**: Background services requiring real-time configuration updates
3. **Configuration reloading behavior**: How each pattern handles appsettings.json changes
4. **Performance implications**: Scoped vs singleton lifetime impacts
5. **Change notification patterns**: Using `OnChange` for reactive configuration updates

## 🚀 Advanced Features to Explore

### 1. **Named Options**
```csharp
// Multiple configurations of same type
services.Configure<DatabaseOptions>("Primary", config.GetSection("Database:Primary"));
services.Configure<DatabaseOptions>("Secondary", config.GetSection("Database:Secondary"));

// Usage
public class DataService
{
    public DataService(IOptionsSnapshot<DatabaseOptions> options)
    {
        var primaryDb = options.Get("Primary");
        var secondaryDb = options.Get("Secondary");
    }
}
```

### 2. **Options Validation**
```csharp
services.Configure<TaxOptions>(config.GetSection("TaxOptions"))
    .Validate(options => options.TaxRate >= 0 && options.TaxRate <= 1, 
              "Tax rate must be between 0 and 1")
    .ValidateOnStart();
```

### 3. **Post-Configure Options**
```csharp
services.PostConfigure<TaxOptions>(options =>
{
    if (options.TaxRate == 0)
        options.TaxRate = 0.05; // Default fallback
});
```

### 4. **Custom Configuration Sources**
```csharp
public class DatabaseConfigurationSource : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder)
        => new DatabaseConfigurationProvider();
}

// Usage
builder.Configuration.Add(new DatabaseConfigurationSource());
```

### 5. **Options Caching with IMemoryCache**
```csharp
public class CachedOptionsService<T> where T : class
{
    private readonly IOptionsMonitor<T> _monitor;
    private readonly IMemoryCache _cache;
    
    public T GetCachedOptions(string key)
    {
        return _cache.GetOrCreate(key, factory =>
        {
            factory.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            return _monitor.CurrentValue;
        });
    }
}
```

### 6. **Environment-Specific Options**
```csharp
services.Configure<ApiOptions>(options =>
{
    if (env.IsDevelopment())
        options.BaseUrl = "https://dev-api.example.com";
    else if (env.IsProduction())
        options.BaseUrl = "https://api.example.com";
});
```

### 7. **Options Pattern with Secrets Manager**
```csharp
builder.Configuration.AddAzureKeyVault(
    new Uri("https://your-keyvault.vault.azure.net/"),
    new DefaultAzureCredential());

// Options automatically populated from Key Vault
services.Configure<ConnectionStrings>(
    builder.Configuration.GetSection("ConnectionStrings"));
```

### 8. **Reactive Options with System.Reactive**
```csharp
public class ReactiveOptionsService<T> where T : class
{
    private readonly IObservable<T> _optionsStream;
    
    public ReactiveOptionsService(IOptionsMonitor<T> monitor)
    {
        _optionsStream = Observable
            .FromEvent<T>(
                h => monitor.OnChange += h,
                h => monitor.OnChange -= h)
            .StartWith(monitor.CurrentValue);
    }
    
    public IObservable<T> OptionsStream => _optionsStream;
}
```

### 9. **Options Snapshot Factory Pattern**
```csharp
public interface IOptionsSnapshotFactory<T> where T : class
{
    T Create(string name = null);
}

public class OptionsSnapshotFactory<T> : IOptionsSnapshotFactory<T> where T : class
{
    private readonly IServiceProvider _serviceProvider;
    
    public T Create(string name = null)
    {
        using var scope = _serviceProvider.CreateScope();
        var options = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<T>>();
        return name == null ? options.Value : options.Get(name);
    }
}
```

### 10. **Configuration Hot Reload with File Watchers**
```csharp
builder.Configuration.AddJsonFile("appsettings.json", 
    optional: false, 
    reloadOnChange: true);

// Custom file watcher for specific config files
services.AddSingleton<IConfigurationChangeWatcher, CustomFileWatcher>();
```

## 📚 Additional Resources

- [Microsoft Docs - Options Pattern](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options)
- [Configuration in .NET](https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration)
- [Dependency Injection in .NET](https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)

## 🤝 Contributing

Feel free to submit issues and enhancement requests!

## 📄 License

This project is for educational purposes.