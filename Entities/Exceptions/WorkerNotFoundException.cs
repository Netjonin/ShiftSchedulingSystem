
namespace Entities.Exceptions;
public class WorkerNotFoundException : NotFoundException
{
    public WorkerNotFoundException(Guid workerId) : base($"The worker with id: {workerId} doesn't exist.")
    {
    }
}
