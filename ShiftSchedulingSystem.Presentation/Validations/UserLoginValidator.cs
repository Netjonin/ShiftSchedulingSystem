using FluentValidation;
using Shared.DataTransferObjects;

namespace ShiftSchedulingSystem.Presentation.Validations;
public class UserLoginValidator : AbstractValidator<UserForAuthenticationDto>
{
    public UserLoginValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(x => x.Password).NotEmpty().WithMessage("{PropertyName} is required");
    }
}
