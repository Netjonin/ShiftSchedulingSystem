
namespace Contracts;

public interface IRepositoryManager
{
    IShiftRepository Shift { get; }
    IWorkerRepository Worker { get; }
    Task SaveAsync();
}
