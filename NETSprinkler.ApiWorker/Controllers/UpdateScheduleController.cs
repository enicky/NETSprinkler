using Microsoft.AspNetCore.Mvc;
using NETSprinkler.ApiWorker.Business.Services.Scheduler;

namespace NETSprinkler.ApiWorker.Controllers;

[ApiController]
public class UpdateScheduleController: Controller
{
    private readonly ILogger<UpdateScheduleController> _logger;
    private readonly ISchedulerService _schedulerService;

    public UpdateScheduleController(ILogger<UpdateScheduleController> logger
        ,ISchedulerService schedulerService)
    {
        _logger = logger;
        _schedulerService = schedulerService;
    }

    [HttpPost("CreateSchedule")]
    public async Task CreateSchedule(CancellationToken cancellationToken, [FromBody] int id)
    {
        await _schedulerService.CreateSchedule(id);
    }
}