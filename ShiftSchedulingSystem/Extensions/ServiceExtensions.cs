using Contracts;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace ShiftSchedulingSystem.Extensions;

public static class ServiceExtensions
{
    public static void AddComponents(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy", builder =>
              builder.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithExposedHeaders("X-Pagination"));
        });

        services.Configure<IISOptions>(options =>
        {
        });

        services.AddSingleton<ILoggerManager, LoggerManager>();

        services.AddScoped<IRepositoryManager, RepositoryManager>();

        services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), options => options.EnableRetryOnFailure()));
    }
}
