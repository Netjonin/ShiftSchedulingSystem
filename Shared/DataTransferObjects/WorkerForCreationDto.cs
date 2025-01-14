
namespace Shared.DataTransferObjects;
public record WorkerForCreationDto
{
    public string Name { get; init; } = string.Empty;
    public int Age { get; init; }
    public string? Department { get; init; }
}
