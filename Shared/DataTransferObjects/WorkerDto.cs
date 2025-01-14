
namespace Shared.DataTransferObjects;
public record WorkerDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; } = string.Empty;
    public int Age { get; init; }
    public string Department { get; init; } = string.Empty;
}
