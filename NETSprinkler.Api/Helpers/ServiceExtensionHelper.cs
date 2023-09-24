using NETSprinkler.Business.DbContext;
using NETSprinkler.Business.Repositories;
using NETSprinkler.Business.Services.Scheduler;

namespace NETSprinkler.Helpers;

public static class ServiceExtensionHelper
{
    public static IServiceCollection RegisterSprinkler(this IServiceCollection s)
    {
        s.AddTransient<ISchedulerService, SchedulerService>();
        s.AddEntityFrameworkSqlServer()
            .AddDbContext<SprinklerDbContext>();
        s.AddScoped(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
        s.AddScoped<IUnitOfWork, UnitOfWork>();
        return s;
    }
}