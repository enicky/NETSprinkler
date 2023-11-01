using NETSprinkler.Business.Services.Scheduler;
using NETSprinkler.Business.Services.Valve;
using NETSprinkler.Common.DbContext;
using NETSprinkler.Common.Repositories;

namespace NETSprinkler.Helpers;

public static class ServiceExtensionHelper
{
    public static IServiceCollection RegisterSprinkler(this IServiceCollection s)
    {
        s.AddTransient<ISchedulerService, SchedulerService>();
        s.AddEntityFrameworkSqlServer()
            .AddDbContext<SprinklerDbContext>();
        s.AddScoped<IValveService, ValveService>();
        s.AddScoped<IValveSettingsService, ValveSettingsService>();
        s.AddScoped(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
        s.AddScoped<IUnitOfWork, UnitOfWork>();
        return s;
    }
}