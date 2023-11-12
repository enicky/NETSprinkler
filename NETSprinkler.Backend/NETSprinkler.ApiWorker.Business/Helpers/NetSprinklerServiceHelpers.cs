using Microsoft.Extensions.DependencyInjection;
using NETSprinkler.ApiWorker.Business.Drivers;
using NETSprinkler.ApiWorker.Business.Jobs;
using NETSprinkler.ApiWorker.Business.Services.Scheduler;
using NETSprinkler.ApiWorker.Business.Services.Sprinkler;
using NETSprinkler.ApiWorker.Business.Services.Valves;
using NETSprinkler.ApiWorker.Business.Settings;
using NETSprinkler.Common.DbContext;
using NETSprinkler.Common.Repositories;
using NETSprinkler.Common.Services;

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
        s.AddScoped<IScheduleService, ScheduleService>();
        s.AddScoped(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
        s.AddScoped<IValveService, ValveService>();
        s.AddSingleton<IGpioDriver, RPiDriver>();
        s.AddTransient<IValveService, ValveService>();
        s.AddTransient<Services.Valve.IValveService, NETSprinkler.ApiWorker.Business.Services.Valve.ValveService>();
        s.AddTransient<Services.Valve.IValveSettingsService, Services.Valve.ValveSettingsService>();
        s.AddTransient<IHangfireScheduleService, HangfireScheduleService>();
        s.AddScoped<ISettingsManager, SettingsManager>();
        //s.AddScoped<IGpioDriver, DummyDriver>();
        return s;
    }
}