using FluentValidation;
using VectorSearchApp.Models;

namespace VectorSearchApp.Validators;

public class QuestionValidator : AbstractValidator<Question>
{
    public QuestionValidator()
    {
        RuleFor(x=>x.Text).NotEmpty().WithMessage("Question text cannot be empty.")
            .MaximumLength(4096).WithMessage("Question text cannot exceed 4096 characters.")
            .WithName("Question Text");

    }
}
