using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Shared.RequestFeatures;

namespace Repository;

public class WorkerRepository : RepositoryBase<Worker>, IWorkerRepository
{
    public WorkerRepository(ApplicationContext ctx) : base(ctx)
    {
    }

    public async Task<PagedList<Worker>> GetWorkersAsync(WorkerParameters parameters, bool trackChanges)
    {
        var workers = await FindAll(trackChanges).OrderBy(x => x.Name)
        .Sort(parameters.OrderBy!)
        .Skip((parameters.PageNumber - 1) * parameters.PageSize)
        .Take(parameters.PageSize)
        .ToListAsync();

        var count = workers.Count();
        return new PagedList<Worker>(workers, count, parameters.PageNumber, parameters.PageSize);
    }

    public async Task<Worker> GetWorkerAsync(Guid id, bool trackChanges) =>
      await FindByCondition(e => e.Id.Equals(id), trackChanges)
        .SingleOrDefaultAsync();

    public void CreateWorker(Worker worker) => Create(worker);

    public void DeleteWorker(Worker worker) => Delete(worker);
}
