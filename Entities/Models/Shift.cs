
namespace Entities.Models;

public class Shift
{
    public Guid Id { get; set; }
    public string ShiftType { get; set; } = string.Empty;
    public string Day { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public Guid WorkerId { get; set; }
    public Worker? worker { get; set; }
}
