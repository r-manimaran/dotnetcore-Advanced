using FluentValidation;
using MinimalApiFilters.Models;

namespace MinimalApiFilters.Validation
{
    public class CustomerValidator:AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x=>x.FirstName) .NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.Now.AddDays(1))
                .WithMessage("Your date of birth cannot be in the futur");
        }
    }
}
