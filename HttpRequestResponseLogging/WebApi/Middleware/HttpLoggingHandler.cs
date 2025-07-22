
using System.Diagnostics;
using System.Text;
using WebApi;

namespace WebApi.Middleware;

public class HttpLoggingHandler : DelegatingHandler
{
    private readonly ILogger<HttpLoggingHandler> _logger;

    public HttpLoggingHandler(ILogger<HttpLoggingHandler> logger)
    {
        _logger = logger;
    }
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var traceId = request.Headers.TryGetValues("X-Trace-Id", out var values) ? values.FirstOrDefault() : Guid.NewGuid().ToString();
        traceId = traceId ?? Guid.NewGuid().ToString();

        var requestBuilder = new StringBuilder();
        var url = $"{request.RequestUri?.Host}{ request.RequestUri?.Port}{ request.RequestUri?.AbsolutePath}";
        var headers = request.Headers.ExceptSensitiveHeaders().Select(x=> $"{x.Key}: {string.Join(", ", x.Value)}]");

        requestBuilder.AppendLine($"[REQUEST]{traceId}");
        requestBuilder.AppendLine($"{request.Method}:{request.RequestUri?.Scheme}://{url}");
        requestBuilder.AppendLine($"Headers:{string.Join(", ",headers)}");
        if(request.Content !=null)
        {
            if(request.Content.Headers.Any())
            {
                var contentHeaders = request.Content.Headers.ExceptSensitiveHeaders().Select(
                    x => $"[{x.Key}: {string.Join(", ", x.Value)}]");

                requestBuilder.AppendLine($"Content Headers: {string.Join(", ", contentHeaders)}");
            }

            if (HttpLoggingHelpers.ResponseCanBeLogged(request.RequestUri?.AbsolutePath))
            {
                requestBuilder.AppendLine("Content:");
                requestBuilder.AppendLine(await request.Content.ReadAsStringAsync(cancellationToken));
            }
        }

        _logger.LogInformation("{Request}", requestBuilder.ToString());
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        var response = await base.SendAsync(request, cancellationToken);
        stopWatch.Stop();

        var responseBuilder = new StringBuilder();  
        responseBuilder.AppendLine($"[RESPONSE]{traceId}");
        responseBuilder.AppendLine($"{request.Method}:{request.RequestUri?.Scheme}://{url} {(int)response.StatusCode} {response.ReasonPhrase} executed in {stopWatch.Elapsed.TotalMilliseconds} ms");
        responseBuilder.AppendLine($"Headers: {string.Join(", ", response.Headers.Select(x => $"[{x.Key}, {string.Join(",", x.Value)}]"))}");

        if(response.Content.Headers.Any())
        {
                       var contentHeaders = response.Content.Headers.Select(
                x => $"[{x.Key}: {string.Join(", ", x.Value)}]");
            responseBuilder.AppendLine($"Content Headers: {string.Join(", ", contentHeaders)}");
        }

        if(HttpLoggingHelpers.ResponseCanBeLogged(request.RequestUri?.AbsolutePath) && HttpLoggingHelpers.ResponseCanBeLogged(request.RequestUri?.AbsolutePath))
        {
            responseBuilder.AppendLine("Content:");
            responseBuilder.AppendLine(await response.Content.ReadAsStringAsync(cancellationToken));
        }

        _logger.LogDebug("{Response}", responseBuilder.ToString());
        _logger.LogDebug("Request completed in {ElapsedTotalMilliseconds}ms", stopWatch.Elapsed.TotalMilliseconds);
        return response;
    }
}
