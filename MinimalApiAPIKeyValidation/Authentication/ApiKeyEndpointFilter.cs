namespace MinimalApiAPIKeyAuthentication.Authentication
{
    public class ApiKeyEndpointFilter : IEndpointFilter
    {
        private readonly IConfiguration _configuration;

        public ApiKeyEndpointFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            if(!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.ApiKeyHeaderName, out var extractedApiKey))
            {
                return new UnauthorizzedHttpObjectResult("Api Key missing"); 
            }
            var apiKey = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName);
            if(!apiKey.Equals(extractedApiKey))
            {
                return new UnauthorizzedHttpObjectResult("Invalid API Key!");
            }
            return await next(context);
        }
    }
}
