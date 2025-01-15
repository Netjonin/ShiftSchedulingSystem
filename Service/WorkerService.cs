using AutoMapper;
using Contracts;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service;

public sealed class WorkerService : IWorkerService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public WorkerService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    public async Task<(IEnumerable<WorkerDto> workers, MetaData metaData)> GetWorkersAsync(WorkerParameters parameters, bool trackChanges)
    {
        var workers = await _repository.Worker.GetWorkersAsync(parameters, trackChanges);
        var workersDto = _mapper.Map<IEnumerable<WorkerDto>>(workers);
        return (workers: workersDto, metaData: workers.MetaData);
    }

    public async Task<WorkerDto> GetWorkerAsync(Guid id, bool trackChanges)
    {
        var worker = await GetWorkerAndCheckIfItExists(id, trackChanges);
        var workerDto = _mapper.Map<WorkerDto>(worker);
        return workerDto;
    }

    public async Task<WorkerDto> CreateWorkerAsync(WorkerForCreationDto workerForCreationDto)
    {
        var worker = _mapper.Map<Worker>(workerForCreationDto);
        _repository.Worker.CreateWorker(worker);
        await _repository.SaveAsync();
        var workerDto = _mapper.Map<WorkerDto>(worker);
        return workerDto;
    }

    public async Task DeleteWorkerAsync(Guid id, bool trackChanges)
    {
        var worker = await GetWorkerAndCheckIfItExists(id, trackChanges);
        _repository.Worker.DeleteWorker(worker);
        await _repository.SaveAsync();
    }

    public async Task UpdateWorkerAsync(Guid id, WorkerForUpdateDto workerForUpdate, bool trackChanges)
    {
        var worker = await GetWorkerAndCheckIfItExists(id, trackChanges);
        _mapper.Map(workerForUpdate, worker);
        await _repository.SaveAsync();
    }

    private async Task<Worker> GetWorkerAndCheckIfItExists(Guid id, bool trackChanges)
    {
        var worker = await _repository.Worker.GetWorkerAsync(id, trackChanges);

        
        return worker;
    }
}
