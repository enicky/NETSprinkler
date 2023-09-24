using Microsoft.Extensions.DependencyInjection;
using NETSprinkler.ApiWorker.Business.Drivers;
using NETSprinkler.ApiWorker.Business.Jobs;
using NETSprinkler.ApiWorker.Business.Services.Scheduler;
using NETSprinkler.ApiWorker.Business.Services.Sprinkler;
using NETSprinkler.ApiWorker.Business.Services.Valves;
using NETSprinkler.Common.DbContext;
using NETSprinkler.Common.Repositories;
using NETSprinkler.Common.Services;
using Serilog;

namespace NETSprinkler.ApiWorker.Business.Helpers;

public static class NetSprinklerServiceHelpers
{
    public static IServiceCollection AddNetSprinkler(this IServiceCollection s)
    {
        s.AddTransient<SampleJob>();
        s.AddTransient<IUnitOfWork, UnitOfWork>();
        s.AddTransient<RunSprinklerJob>();
        s.AddScoped<ISprinklerService, SprinklerService>();
        s.AddScoped<ICronScheduleService, CronScheduleService>();
        s.AddScoped<ISchedulerService, SchedulerService>();
        s.AddScoped(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
        s.AddScoped<IValveService, ValveService>();
        //s.AddScoped<IGpioDriver, RPiDriver>();
        s.AddScoped<IGpioDriver, DummyDriver>();
        return s;
    }
}