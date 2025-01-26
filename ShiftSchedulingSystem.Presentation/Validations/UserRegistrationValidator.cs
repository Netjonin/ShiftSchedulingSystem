
using FluentValidation;
using Shared.DataTransferObjects;

namespace ShiftSchedulingSystem.Presentation.Validations;

public class UserRegistrationValidator : AbstractValidator<UserForRegistrationDto>
{
    public UserRegistrationValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("{PropertyName} is required");
    }
}
