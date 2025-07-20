using Microsoft.AspNetCore.HttpLogging;

namespace WebApi.Interceptors
{
    public class SensitiveDataReductionInterceptor : IHttpLoggingInterceptor
    {
        public ValueTask OnRequestAsync(HttpLoggingInterceptorContext logContext)
        {
            if(logContext.HttpContext.Request.Method == "POST")
            {
                // Don't log anything for POST requests
                logContext.LoggingFields = HttpLoggingFields.None;
            }

            // Don't enrich if we are not going to log any part of the request
            if (!logContext.IsAnyEnabled(HttpLoggingFields.Request))
            {
                return default;
            }

            if (logContext.TryDisable(HttpLoggingFields.RequestPath))
            {
                RedactPath(logContext);
            }

            if (logContext.TryDisable(HttpLoggingFields.RequestHeaders))
            {
                RedactRequestHeaders(logContext);
            }
            EnrichRequest(logContext);

            return default;
        }

        private void EnrichRequest(HttpLoggingInterceptorContext logContext)
        {
            logContext.AddParameter("new-request-field", Guid.NewGuid().ToString());
        }

        private void RedactRequestHeaders(HttpLoggingInterceptorContext logContext)
        {
            foreach (var header in logContext.HttpContext.Request.Headers)
            {
                logContext.AddParameter(header.Key, "REDACTED");
            }
        }

        private void RedactPath(HttpLoggingInterceptorContext logContext)
        {
            logContext.AddParameter(nameof(HttpLoggingInterceptorContext.HttpContext.Request.Path), "REDACTED");
        }

        public ValueTask OnResponseAsync(HttpLoggingInterceptorContext logContext)
        {
            if(!logContext.IsAnyEnabled(HttpLoggingFields.Response))
            {
                return default;
            }

            if(logContext.TryDisable(HttpLoggingFields.ResponseHeaders))
            {
                RedactResponseHeaders(logContext);
            }
            EnrichResponse(logContext);
            return default;
        }

        private void EnrichResponse(HttpLoggingInterceptorContext logContext)
        {
            logContext.AddParameter("new-response-field", Guid.NewGuid().ToString());
        }

        private void RedactResponseHeaders(HttpLoggingInterceptorContext logContext)
        {
            foreach( var header in logContext.HttpContext.Response.Headers)
            {               
               logContext.AddParameter(header.Key, "REDACTED");                
            }
        }
    }
}
