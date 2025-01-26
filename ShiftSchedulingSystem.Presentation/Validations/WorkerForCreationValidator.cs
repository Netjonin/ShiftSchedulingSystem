using FluentValidation;
using Shared.DataTransferObjects;

namespace ShiftSchedulingSystem.Presentation.Validations;

public class WorkerForCreationValidator : AbstractValidator<WorkerForCreationDto>
{
    public WorkerForCreationValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} is required");
        RuleFor(x => x.Department).NotEmpty().WithMessage("{PropertyName} is required");
        //RuleForEach(x => x.shifts)
        //    .SetValidator(new ShiftForCreationValidator());
    }
}
