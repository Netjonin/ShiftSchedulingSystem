using Contracts;
using Entities.ConfigurationModels;
using Entities.Models;
using LoggerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Service;
using Service.Contracts;
using ShiftSchedulingSystem.Presentation.ActionFilters;
using System.Net;
using System.Text;
using System.Threading.RateLimiting;

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

        services.AddScoped<IServiceManager, ServiceManager>();

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddScoped<ValidationFilterAttribute>();

        services.AddControllers(config =>
        {
            config.RespectBrowserAcceptHeader = true;
            config.ReturnHttpNotAcceptable = true;
            config.CacheProfiles.Add("120SecondsDuration", new CacheProfile
            {
                Duration = 120
            });


        }).AddXmlDataContractSerializerFormatters()
            .AddApplicationPart(typeof(ShiftSchedulingSystem.Presentation.AssemblyReference).Assembly);
        //services.AddOutputCache(opt =>
        //{
        //    //opt.AddBasePolicy(bp => bp.Expire(TimeSpan.FromSeconds(10)));
        //    opt.AddPolicy("120SecondsDuration", p => p.Expire(TimeSpan.FromSeconds(120)));
        //});

        var builder = services.AddIdentity<User, IdentityRole>(opt =>
        {
            opt.Password.RequireDigit = true;
            opt.Password.RequireLowercase = false;
            opt.Password.RequireUppercase = false;
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequiredLength = 10;
            opt.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationContext>()
        .AddDefaultTokenProviders();


        services.AddAutoMapper(typeof(Program));
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddResponseCaching();
        services.AddOutputCache();

        services.AddRateLimiter(opt =>
        {
            opt.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            RateLimitPartition.GetFixedWindowLimiter("GlobalLimiter",
            partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 30,
                QueueLimit = 2,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                Window = TimeSpan.FromMinutes(1)
            }));

            opt.AddPolicy("SpecificPolicy", context =>
              RateLimitPartition.GetFixedWindowLimiter("SpecificLimiter",
               partition => new FixedWindowRateLimiterOptions
               {
                   AutoReplenishment = true,
                   PermitLimit = 3,
                   Window = TimeSpan.FromSeconds(10)
               }));

            opt.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    await context.HttpContext.Response
                    .WriteAsync($"Too many requests. Please try again after {retryAfter.TotalSeconds} second(s).", token);
                else
                    await context.HttpContext.Response
                    .WriteAsync("Too many requests. Please try again later.", token);
            };
        });

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = configuration.GetSection("JwtSettings")["validIssuer"],
                ValidAudience = configuration.GetSection("JwtSettings")["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JwtSettings")["key"]!))

            };
        });
        services.Configure<JwtConfiguration>(configuration.GetSection("JwtSettings"));
    }
}
