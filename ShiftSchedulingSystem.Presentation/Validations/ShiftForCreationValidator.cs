using FluentValidation;
using Shared.DataTransferObjects;

namespace ShiftSchedulingSystem.Presentation.Validations;

public class ShiftForCreationValidator : AbstractValidator<ShiftForCreationDto>
{
    public ShiftForCreationValidator()
    {
        RuleFor(x => x.ShiftType)
        .NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(x => x.Day)
            .NotNull().NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(9).WithMessage("{PropertyName} must not be more than 9 characters");
        RuleFor(x => x.StartTime).NotEmpty().WithMessage("{PropertyName} is required");

        RuleFor(x => x.EndTime).NotEmpty().WithMessage("{PropertyName} is required");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .GreaterThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage("{PropertyName} must be greater than the current date");
    }
}
