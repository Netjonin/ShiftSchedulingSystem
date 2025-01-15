using AutoMapper;
using Contracts;
using Entities.ConfigurationModels;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Service.Contracts;

namespace Service;
public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IShiftService> _shiftService;
    private readonly Lazy<IWorkerService> _workerService;
    private readonly Lazy<IAuthenticationService> _authenticationService;


    public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper,
        UserManager<User> userManager, IOptions<JwtConfiguration> configuration)
    {
        _shiftService = new Lazy<IShiftService>(() => new ShiftService(repositoryManager, logger, mapper));
        _workerService = new Lazy<IWorkerService>(() => new WorkerService(repositoryManager, logger, mapper));
        _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(logger, mapper, userManager, configuration));
    }

    public IShiftService ShiftService => _shiftService.Value;
    public IWorkerService WorkerService => _workerService.Value;
    public IAuthenticationService AuthenticationService => _authenticationService.Value;

}
