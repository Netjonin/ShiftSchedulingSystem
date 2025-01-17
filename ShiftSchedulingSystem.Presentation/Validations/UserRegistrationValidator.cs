
using FluentValidation;
using Shared.DataTransferObjects;

namespace ShiftSchedulingSystem.Presentation.Validations;

public class UserRegistrationValidator : AbstractValidator<UserForRegistrationDto>
{
    public UserRegistrationValidator()
    {
        RuleFor(x => x.FirstName).NotNull().WithMessage("{PropertyName} is required");
        RuleFor(x => x.LastName).NotNull().WithMessage("{PropertyName} is required");
    }
}
