
namespace Shared.DataTransferObjects;

public record ShiftForCreationDto
{
    public string? ShiftType { get; init; }
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }
    public string Day { get; init; }
    public DateOnly Date { get; init; }
}
