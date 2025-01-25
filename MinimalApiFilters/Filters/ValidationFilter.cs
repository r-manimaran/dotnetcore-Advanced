using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace MinimalApiFilters.Filters
{
    public class ValidationFilter<T> : IEndpointFilter where T : class
    {
        private readonly IValidator<T> _validator;
        public ValidationFilter(IValidator<T> validator)
        {
            _validator = validator;
        }

        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
           

            var argument = context.Arguments.OfType<T>()
                            .FirstOrDefault();
            if (argument is null)
            {
                return Results.BadRequest("Invalid request payload");
            }
            var validationResult = await _validator.ValidateAsync(argument);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }
          

            //after endpoint call

            return await next(context);
        }
    }
}
