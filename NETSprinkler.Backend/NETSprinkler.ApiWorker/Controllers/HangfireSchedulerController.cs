using Microsoft.AspNetCore.Mvc;
using NETSprinkler.ApiWorker.Business.Services.Scheduler;
using NETSprinkler.ApiWorker.Business.Services.Sprinkler;

namespace NETSprinkler.ApiWorker.Controllers;

[ApiController]
public class HangfireSchedulerController: Controller
{
    private readonly ILogger<HangfireSchedulerController> _logger;
    private readonly IHangfireScheduleService _schedulerService;
    private readonly ISprinklerService _sprinklerService;

    public HangfireSchedulerController(ILogger<HangfireSchedulerController> logger
        , IHangfireScheduleService schedulerService,
        ISprinklerService sprinklerService)
    {
        _logger = logger;
        _schedulerService = schedulerService;
        _sprinklerService = sprinklerService;
    }

    [HttpPost("EnableHangfireSchedule")]
    public async Task<OkResult> EnableHangfireSchedule(CancellationToken cancellationToken, [FromBody] int id)
    {
        _logger.LogDebug("[HangfireSchedulerController:EnableHangfireSchedule] start enabling HangFire schedule by id {id}", id);
        await _schedulerService.CreateSchedule(id, cancellationToken).ConfigureAwait(false);
        return new OkResult();
    }

    [HttpDelete("DisableHangfireSchedule")]
    public async Task<OkResult> DisableHangfireSchedule(CancellationToken cancellation, int id)
    {
        _logger.LogDebug("[HangfireSchedulerController:DisableHangfireSchedule] Start removing HangFire schedule by id {id}", id);
        await _schedulerService.DeleteSchedule(id, cancellation);

        return new OkResult();
    }

    [HttpGet("GetAllHangfireSchedules")]
    public async Task<List<string>> GetAllHangfireSchedules(CancellationToken cancellationToken)
    {
        _logger.LogDebug("[HangfireSchedulerController:GetAllHangfireSchedules] Retrieve all HangfireEnabled Schedules");
        var result = await _schedulerService.GetAllHangfireSchedules(cancellationToken).ConfigureAwait(false);
        return result;
    }

    [HttpPost("test")]
    public async Task Test(CancellationToken token, int id, int status)
    {
        _logger.LogDebug("[HangfireSchedulerController:Test] Start Testing Output of ValveController. Set {id} to status {status}", id, status);
        switch (status)
        {
            case 0:
                await _sprinklerService.StartAsync(id);
                break;
            case 1:
                await _sprinklerService.StopAsync(id);
                break;
        }
        
    }
}