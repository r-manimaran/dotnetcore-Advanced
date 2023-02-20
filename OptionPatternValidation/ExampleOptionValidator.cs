using FluentValidation;

namespace OptionPatternValidation
{
    public class ExampleOptionValidator:AbstractValidator<ExampleOptions>
    {
        public ExampleOptionValidator()
        {
            RuleFor(x => x.Level).IsInEnum();

            RuleFor(x => x.RetryCount).InclusiveBetween(1, 9);
        }
    }
}
