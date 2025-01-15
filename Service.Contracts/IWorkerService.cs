using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service.Contracts;
public interface IWorkerService
{
    Task<(IEnumerable<WorkerDto> workers, MetaData metaData)> GetWorkersAsync(WorkerParameters parameters, bool trackChanges);
    Task<WorkerDto> GetWorkerAsync(Guid id, bool trackChanges);
    Task<WorkerDto> CreateWorkerAsync(WorkerForCreationDto workerForCreationDto);
    Task DeleteWorkerAsync(Guid id, bool trackChanges);
    Task UpdateWorkerAsync(Guid id, WorkerForUpdateDto workerForUpdate,
        bool trackChanges);

}
