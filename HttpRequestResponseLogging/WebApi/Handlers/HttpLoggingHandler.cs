using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace WebApi.Handlers
{
    public class HttpLoggingHandler : DelegatingHandler
    {
        private readonly ILogger<HttpLoggingHandler> _logger;

        public HttpLoggingHandler(ILogger<HttpLoggingHandler> logger)
        {
            _logger = logger;
            InnerHandler = new HttpClientHandler();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Log the request
            _logger.LogInformation($"Request: {request.Method} {request.RequestUri}");
            
            if (request.Content != null)
            {
                var requestContent = await request.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogInformation($"Request Content: {requestContent}");
            }

            // Send the request and get the response
            var response = await base.SendAsync(request, cancellationToken);

            // Log the response
            _logger.LogInformation($"Response: {(int)response.StatusCode} {response.StatusCode}");
            
            if (response.Content != null)
            {
                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogInformation($"Response Content: {responseContent}");
            }

            return response;
        }
    }
}