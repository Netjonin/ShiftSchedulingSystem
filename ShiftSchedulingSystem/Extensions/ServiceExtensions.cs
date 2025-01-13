using Contracts;
using LoggerService;

namespace ShiftSchedulingSystem.Extensions;

public static class ServiceExtensions
{
    public static void AddComponents(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ILoggerManager, LoggerManager>();
    }
}
