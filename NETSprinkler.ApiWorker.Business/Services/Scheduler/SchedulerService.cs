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
        AddRecurringJobs(GenerateJobIds(id), id, await _cronScheduleService.GenerateCronStrings(registeredSchedule));
    }

    
    public async Task DeleteSchedule(int id, CancellationToken cancellation = default)
    {
        var registeredSchedule = await GetById(id);
        if (registeredSchedule == null) throw new InvalidScheduleIdException();
        _logger.LogInformation($"[SchedulerService:DeleteSchedule] Start deleting schedule based on id {id}");
        var (startJobId, endJobId) = GenerateJobIds(id);
        _jobManager.RemoveIfExists(startJobId);
        _jobManager.RemoveIfExists(endJobId);
        _logger.LogInformation($"[SchedulerService:DeleteSchedule] Finished removing schedules based on id {id}");
    }

    public Task<Schedule?> GetScheduleById(int id)
    {
        var res = GetById(id);
        if (res == null) throw new InvalidScheduleIdException();
        _logger.LogInformation("[SchedulerService:GetScheduleById] retrieved Schedule {Id}", id);
        return res;
    }

    private static (string, string) GenerateJobIds(int id)
    {
        return ($"job_{id}_start", $"job_{id}_end");
    }

    private void AddRecurringJobs((string, string) jobIds, int id, (string, string) cronStrings)
    {
        _logger.LogInformation("[SchedulerService:CreateSchedule] Start Schedule crontab - {StartCronString} - ValveId {ValveId}", cronStrings.Item1, id);
        _logger.LogInformation("[SchedulerService:CreateSchedule] End Schedule crontab - {EndCronString} - ValveId {ValveId}", cronStrings.Item2, id);

        _jobManager.AddOrUpdate<RunSprinklerJob>(jobIds.Item1, x => x.RunAsync(id, true), cronStrings.Item1, new RecurringJobOptions()
        {
            TimeZone = TimeZoneInfo.Local
        });
        _jobManager.AddOrUpdate<RunSprinklerJob>(jobIds.Item2, x => x.RunAsync(id, false), cronStrings.Item2, new RecurringJobOptions()
        {
            TimeZone = TimeZoneInfo.Local
        });

    }

}