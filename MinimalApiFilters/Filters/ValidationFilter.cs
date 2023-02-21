using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace MinimalApiFilters.Filters
{
    public class ValidationFilter<T> : IRouteHandlerFilter where T : class
    {
        private readonly IValidator<T> _validator;
        public ValidationFilter(IValidator<T> validator)
        {
            _validator = validator;
        }

        public async ValueTask<object> InvokeAsync(
            RouteHandlerInvocationContext context,
            RouteHandlerFilterDelegate next)
        {

            //before endpoint call
            var result = await next(context);

            //after endpoint call

            return await next(context);
        }
    }
}
