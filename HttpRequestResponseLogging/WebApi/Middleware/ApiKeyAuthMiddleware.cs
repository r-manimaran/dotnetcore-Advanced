namespace WebApi.Middleware;

public class ApiKeyAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private const string API_KEY_HEADER = "X-API-Key";
    public ApiKeyAuthMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        //skip authentication for the specific paths like swagger and openapi
        if(context.Request.Path.StartsWithSegments("/swagger") ||
            context.Request.Path.StartsWithSegments("/openapi"))
        {
            await _next(context);
            return;
        }

        // Check if API key is provided
        if(!context.Request.Headers.TryGetValue(API_KEY_HEADER, out var extractedApiKey)){
            context.Response.StatusCode = StatusCodes.Status401Unauthorized; // Unauthorized
            await context.Response.WriteAsync("API Key is missing or invalid.");
            return;
        }

        // Validate the API key
        var apiKey = _configuration.GetValue<string>("ApiKey");
        if(!apiKey.Equals(extractedApiKey, StringComparison.OrdinalIgnoreCase))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden; // Forbidden
            await context.Response.WriteAsync("Forbidden: Invalid API Key.");
            return;
        }
        await _next(context);
    }
}

public static class ApiKeyAuthMiddlewareExtensions
{
    public static IApplicationBuilder UseApiKeyAuth(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ApiKeyAuthMiddleware>();
    }
}
