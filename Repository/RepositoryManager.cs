using Contracts;

namespace Repository;

public class RepositoryManager : IRepositoryManager
{
    private readonly ApplicationContext _ctx;
    private readonly Lazy<IShiftRepository> _ShiftRepository;
    private readonly Lazy<IWorkerRepository> _workerRepository;

    public RepositoryManager(ApplicationContext ctx)
    {
        _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        _ShiftRepository = new Lazy<IShiftRepository>(() => new ShiftRepository(ctx));
        _workerRepository = new Lazy<IWorkerRepository>(() => new WorkerRepository(ctx));
    }
    public IShiftRepository Shift => _ShiftRepository.Value;
    public IWorkerRepository Worker => _workerRepository.Value;
    public async Task SaveAsync() => await _ctx.SaveChangesAsync();
}
