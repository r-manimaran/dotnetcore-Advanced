using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using System.Threading.RateLimiting;

namespace WebApi;

public static class AppExtensions
{
    public static void ConfigureSecurity(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.AddAuthorization();
    }

    public static void ConfigureRateLimiting(this IServiceCollection services)
    {
        // Configure rate limiting
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests; // Set rejection status code

            options.OnRejected = async (context, cancellationToken) =>
             {
                 if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out TimeSpan retryAfter))
                 {
                     context.HttpContext.Response.Headers.RetryAfter = $"{retryAfter.TotalSeconds}";
                     
                     ProblemDetailsFactory problemDetailsFactory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();

                     Microsoft.AspNetCore.Mvc.ProblemDetails problemDetails = problemDetailsFactory.CreateProblemDetails(
                                      context.HttpContext, StatusCodes.Status429TooManyRequests, "Too many Request",
                                      detail: $"Too many requests. Please try again after {retryAfter.TotalSeconds} Seconds");
                     
                     context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                     
                     await context.HttpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
                 }
             };
         
            options.AddFixedWindowLimiter("fixed", opt =>
            {
                opt.PermitLimit = 5; // Allow 5 requests
                opt.Window = TimeSpan.FromMinutes(1); // Per minute
            });

            options.AddPolicy("per-user", httpContext =>
            {
                string? userId = httpContext.User.FindFirstValue("userId");
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    // for authenticated users, use the bucketLimiter
                    return RateLimitPartition.GetTokenBucketLimiter(userId, _
                        => new TokenBucketRateLimiterOptions
                        {
                            TokenLimit = 5, // Allow 5 requests
                            TokensPerPeriod = 2, // Allow 2 requests per period
                            ReplenishmentPeriod = TimeSpan.FromMinutes(1) // Replenish every minute
                            
                        });
                 }

                // for anonymous users, use the fixed window limiter
                return RateLimitPartition.GetFixedWindowLimiter("anonymous", _
                    => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 5, // Allow 5 requests
                        Window = TimeSpan.FromMinutes(1) // Per minute
                    });
            });
        });
    }

    public static void MapSecurityMiddleware(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }

    public static void MapRateLimitingMiddleware(this IApplicationBuilder app)
    {
        app.UseRateLimiter();
    }
}
