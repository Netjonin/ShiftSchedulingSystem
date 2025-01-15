
namespace Service.Contracts;
public interface IServiceManager
{
    IShiftService ShiftService { get; }
    IWorkerService WorkerService { get; }
    IAuthenticationService AuthenticationService { get; }
}
