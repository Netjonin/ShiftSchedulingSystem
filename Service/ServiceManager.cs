using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Service.Contracts;

namespace Service;
public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IShiftService> _shiftService;
    private readonly Lazy<IWorkerService> _workerService;
    

    public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper,
        UserManager<User> userManager)
    {
        _shiftService = new Lazy<IShiftService>(() => new ShiftService(repositoryManager, logger, mapper));
        _workerService = new Lazy<IWorkerService>(() => new WorkerService(repositoryManager, logger, mapper));
    }

    public IShiftService ShiftService => _shiftService.Value;
    public IWorkerService WorkerService => _workerService.Value;
    
}
