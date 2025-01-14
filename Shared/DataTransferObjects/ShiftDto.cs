namespace Shared.DataTransferObjects;
public record ShiftDto
{
    public Guid Id { get; init; }
    public string ShiftType { get; init; } = string.Empty;
    public string Day { get; init; } = string.Empty;
    public DateOnly Date { get; init; }
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }
}
