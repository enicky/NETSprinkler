using Hangfire.Console.Extensions;
using Microsoft.Extensions.Logging;
using NETSprinkler.ApiWorker.Business.Services.Scheduler;
using NETSprinkler.ApiWorker.Business.Services.Sprinkler;
using NETSprinkler.ApiWorker.Business.Services.Valves;
using NETSprinkler.Common.Services;

namespace NETSprinkler.ApiWorker.Business.Jobs;

public class RunSprinklerJob
{
    private readonly ILogger<RunSprinklerJob> _logger;
    private readonly IJobManager _jobManager;
    private readonly ISchedulerService _schedulerService;
    private readonly ISprinklerService _sprinklerService;
    private readonly IValveService _valveService;

    public RunSprinklerJob(ILogger<RunSprinklerJob> logger, IJobManager jobManager, ISchedulerService schedulerService,
        ISprinklerService sprinklerService, IValveService valveService)
    {
        _logger = logger;
        _jobManager = jobManager;
        _schedulerService = schedulerService;
        _sprinklerService = sprinklerService;
        _valveService = valveService;
    }

    public async Task RunAsync(int jobId, bool isStart = true)
    {
        _logger.LogInformation("[RunSprinklerJob::RunAsync] Start processing of job {JobId}", jobId);
        var retrievedJob = await _schedulerService.GetScheduleById(jobId).ConfigureAwait(false);
        if (retrievedJob != null && retrievedJob.SprinklerValveId.HasValue)
        {
            // Start running sprinkler Job ... 
            if (isStart)
            {
                await _sprinklerService.StartAsync(retrievedJob.SprinklerValveId.Value).ConfigureAwait(false);
            }
            else
            {
                await _sprinklerService.StopAsync(retrievedJob.SprinklerValveId.Value).ConfigureAwait(false);
            }
        }
    }
}