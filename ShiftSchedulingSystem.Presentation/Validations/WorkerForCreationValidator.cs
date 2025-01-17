using FluentValidation;
using Shared.DataTransferObjects;

namespace ShiftSchedulingSystem.Presentation.Validations;

public class WorkerForCreationValidator : AbstractValidator<WorkerForCreationDto>
{
    public WorkerForCreationValidator()
    {
        RuleFor(x => x.Name).NotNull().WithMessage("{PropertyName} is required");
        RuleFor(x => x.Department).NotNull().WithMessage("{PropertyName} is required");
        //RuleForEach(x => x.shifts)
        //    .SetValidator(new ShiftForCreationValidator());
    }
}
