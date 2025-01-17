using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using Repository.Seeds;
using ShiftSchedulingSystem.Extensions;

var builder = WebApplication.CreateBuilder(args);

LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

builder.Services.AddAuthentication();
builder.Services.AddComponents(builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();
app.UseExceptionHandler(opt => { });
// Configure the HTTP request pipeline.

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

if (app.Environment.IsProduction())
    app.UseHsts();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseRateLimiter();
app.UseCors("CorsPolicy");
app.UseResponseCaching();
//app.UseOutputCache();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
