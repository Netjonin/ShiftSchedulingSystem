using Contracts;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace ShiftSchedulingSystem.Extensions;

public static class ServiceExtensions
{
    public static void AddComponents(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ILoggerManager, LoggerManager>();

        services.AddScoped<IRepositoryManager, RepositoryManager>();

        services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), options => options.EnableRetryOnFailure()));
    }
}
