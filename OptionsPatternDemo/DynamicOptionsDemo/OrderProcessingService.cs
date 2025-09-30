
using DynamicOptionsDemo.Options;
using Microsoft.Extensions.Options;
using LogLevel = DynamicOptionsDemo.Options.LogLevel;

namespace DynamicOptionsDemo;

public class OrderProcessingService : BackgroundService
{
    private LoggingOptions loggingOptions;
    private readonly ILogger<OrderProcessingService> _logger;
    public OrderProcessingService(IOptionsMonitor<LoggingOptions> optionsMonitor, ILogger<OrderProcessingService> logger)
    {
        _logger = logger;

        loggingOptions = optionsMonitor.CurrentValue;
        
        optionsMonitor.OnChange((options) =>
        {
            loggingOptions = options;
            _logger.LogInformation("[Config Change] LogLevel changed to {LogLevel}",loggingOptions.LogLevel);
        });
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Log(Options.LogLevel.Information,($"[Order Processing] Processing order at {DateTime.Now} with LogLevel {loggingOptions.LogLevel}"));

            Log(LogLevel.Debug, "Debug message: background check-in.");
            Log(LogLevel.Information, "Info message: service running.");
            Log(LogLevel.Warning, "Warning message: potential issue detected.");
            Log(LogLevel.Error, "Error message: something went wrong.");


            await Task.Delay(5000, stoppingToken);
        }
    }
    private void Log(LogLevel level, string message) 
    {
        if ((int)level < (int) loggingOptions.LogLevel)
            return;

        switch (level)
        {
            case LogLevel.Debug:
                _logger.LogDebug(message);
                break;
            case LogLevel.Information:
                _logger.LogInformation(message);
                break;
            case LogLevel.Warning:
                _logger.LogWarning(message);
                break;
            ;
            case LogLevel.Error:
                _logger.LogError(message);
             break;

        }

    }
}
