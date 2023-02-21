namespace MinimalApiAPIKeyAuthentication
{
    public class UnauthorizzedHttpObjectResult : IResult, IStatusCodeHttpResult
    {
        private readonly object _body;

        public UnauthorizzedHttpObjectResult(object body)
        {
            _body = body;
        }
        public int? StatusCode => StatusCodes.Status401Unauthorized;

        int? IStatusCodeHttpResult.StatusCode => StatusCode;

        public async Task ExecuteAsync(HttpContext httpContext)
        {
            ArgumentNullException.ThrowIfNull(httpContext);
            httpContext.Response.StatusCode = (int)StatusCode;

            if(_body is string s)
            {
                await httpContext.Response.WriteAsync(s);
                return;
            }
            await httpContext.Response.WriteAsJsonAsync(_body);
        }
    }
}
