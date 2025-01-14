namespace Shared.DataTransferObjects;

public record ShiftForUpdateDto
{
    public string ShiftType { get; init; } = string.Empty;
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }
    public string Day { get; init; } = string.Empty;
    public DateOnly Date { get; init; }
}

