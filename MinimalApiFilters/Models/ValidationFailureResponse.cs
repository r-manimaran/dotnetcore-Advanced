using FluentValidation.Results;
using System.Runtime.CompilerServices;

namespace MinimalApiFilters.Models
{
    public class ValidationFailureResponse
    {
        public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();
    }

    public static class ValidationFailureMapper
    {
        public static ValidationFailureResponse(this IEnumerable<ValidationFailure> failure)
        {
            return new ValidationFailureResponse
            {
                Errors = failure.Select(x => x.ErrorMessage)
            };
        }
    }
}
