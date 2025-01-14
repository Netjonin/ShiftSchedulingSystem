namespace Entities.Models;

public class Worker
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public int Age { get; set; }
    public ICollection<Shift>? shifts { get; set; }
}