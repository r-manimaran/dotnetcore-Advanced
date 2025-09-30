namespace DynamicOptionsDemo.Options;

public class LoggingOptions
{
    public const string SectionName = nameof(LoggingOptions);
    public LogLevel LogLevel { get; set; } = LogLevel.Information;
}

public enum LogLevel
{
    None = 0,
    Trace =1,
    Debug=2,
    Information=3,
    Warning=4,
    Error=5,
    Critical=6
    
}
