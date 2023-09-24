using Hangfire;
using Microsoft.Extensions.Logging;
using NETSprinkler.ApiWorker.Business.Jobs;
using NETSprinkler.Common.Exceptions;
using NETSprinkler.Common.Repositories;
using NETSprinkler.Common.Services;
using NETSprinkler.Models.Entity.Schedule;

namespace NETSprinkler.ApiWorker.Business.Services.Scheduler;

public class SchedulerService : ServiceAsync<Schedule>, ISchedulerService
{
    private readonly ILogger<SchedulerService> _logger;
    private readonly IRepositoryAsync<Schedule> _repositoryAsync;
    private readonly IRecurringJobManager _jobManager;
    private readonly ICronScheduleService _cronScheduleService;

    public SchedulerService(ILogger<SchedulerService> logger, IRepositoryAsync<Schedule> repositoryAsync,
        IRecurringJobManager jobManager,
        ICronScheduleService cronScheduleService)
        : base(repositoryAsync)
    {
        _logger = logger;
        _repositoryAsync = repositoryAsync;
        _jobManager = jobManager;
        _cronScheduleService = cronScheduleService;
    }

    public async Task CreateSchedule(int id, CancellationToken token = default)
    {
        var registeredSchedule = await GetById(id);// _repositoryAsync.GetById(id);
        if (registeredSchedule == null) throw new InvalidScheduleIdException();
        _logger.LogInformation("[SchedulerService:CreateSchedule] Start creating a schedule based on id {Id}", id);
        var startCronString = await _cronScheduleService.CreateCronString(registeredSchedule);
        var endCronString = await _cronScheduleService.CreateCronString(registeredSchedule, true);
        _logger.LogInformation("[SchedulerService:CreateSchedule] Start Schedule crontab - {StartCronString} - ValveId {ValveId}", startCronString, registeredSchedule.SprinklerValveId);
        _logger.LogInformation("[SchedulerService:CreateSchedule] End Schedule crontab - {EndCronString} - ValveId {ValveId}", endCronString, registeredSchedule.SprinklerValveId);
        _jobManager.AddOrUpdate<RunSprinklerJob>($"job_{id}_start", x => x.RunAsync(id, true), startCronString, new RecurringJobOptions()
        {
            TimeZone = TimeZoneInfo.Local
        });
        _jobManager.AddOrUpdate<RunSprinklerJob>($"job_{id}_end", x => x.RunAsync(id, false), endCronString, new RecurringJobOptions()
        {
            TimeZone = TimeZoneInfo.Local
        });
    }

    public Task<Schedule?> GetScheduleById(int id)
    {
        var res = GetById(id);
        if (res == null) throw new InvalidScheduleIdException();
        _logger.LogInformation("[SchedulerService:GetScheduleById] retrieved Schedule {Id}", id);
        return res;
    }
}