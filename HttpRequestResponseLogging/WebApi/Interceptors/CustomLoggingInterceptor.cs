using Microsoft.AspNetCore.HttpLogging;

namespace WebApi.Interceptors;

public class CustomLoggingInterceptor : IHttpLoggingInterceptor
{
    /// <summary>
    /// This gets called right before the request data is logged, allowing you to customize requests logs.
    /// </summary>
    /// <param name="logContext"></param>
    /// <returns></returns>
    public ValueTask OnRequestAsync(HttpLoggingInterceptorContext logContext)
    {
        // Remove specific Header from logged.
        logContext.HttpContext.Request.Headers.Remove("X-API-Key");

        // Add Custom information to log
        logContext.AddParameter("RequestId",Guid.NewGuid().ToString());

        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Allow to adjust the response logs
    /// </summary>
    /// <param name="logContext"></param>
    /// <returns></returns>
    public ValueTask OnResponseAsync(HttpLoggingInterceptorContext logContext)
    {
        // Remove sensitive response header
        logContext.HttpContext.Response.Headers.Remove("Set-Cookie");

        // Log Additional context
        logContext.AddParameter("new-response-field", Guid.NewGuid().ToString());

        return ValueTask.CompletedTask;
    }
}
