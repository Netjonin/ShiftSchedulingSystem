using FluentValidation;
using Shared.DataTransferObjects;

namespace ShiftSchedulingSystem.Presentation.Validations;
public class ShiftForUpdateValidator : AbstractValidator<ShiftForUpdateDto>
{
    public ShiftForUpdateValidator()
    {
        RuleFor(x => x.ShiftType)
            .NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(x => x.Day)
            .NotNull().NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(9).WithMessage("{PropertyName} must not be more than 9 characters");
        RuleFor(x => x.StartTime).NotNull().WithMessage("{PropertyName} is required");

        RuleFor(x => x.EndTime).NotEmpty().WithMessage("{PropertyName} is required");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("{PropertyName} is required");
    }
}