
namespace Shared.DataTransferObjects;
public record WorkerForUpdateDto
{
    public string Name { get; init; } = string.Empty;
    public int Age { get; init; }
    public string? Department { get; init; }
}