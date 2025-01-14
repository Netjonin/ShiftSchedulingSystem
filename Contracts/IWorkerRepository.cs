using Entities.Models;
using Shared.RequestFeatures;

namespace Contracts;
public interface IWorkerRepository
{
    Task<PagedList<Worker>> GetWorkersAsync(WorkerParameters parameters, bool trackChanges);
    Task<Worker> GetWorkerAsync(Guid id, bool trackChanges);
    void CreateWorker(Worker worker);
    void DeleteWorker(Worker worker);
}
