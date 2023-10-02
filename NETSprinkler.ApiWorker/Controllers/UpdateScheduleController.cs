using Microsoft.AspNetCore.Mvc;
using NETSprinkler.ApiWorker.Business.Services.Scheduler;
using NETSprinkler.ApiWorker.Business.Services.Sprinkler;

namespace NETSprinkler.ApiWorker.Controllers;

[ApiController]
public class UpdateScheduleController: Controller
{
    private readonly ILogger<UpdateScheduleController> _logger;
    private readonly ISchedulerService _schedulerService;
    private readonly ISprinklerService _sprinklerService;

    public UpdateScheduleController(ILogger<UpdateScheduleController> logger
        ,ISchedulerService schedulerService,
        ISprinklerService sprinklerService)
    {
        _logger = logger;
        _schedulerService = schedulerService;
        _sprinklerService = sprinklerService;
    }

    [HttpPost("CreateSchedule")]
    public async Task CreateSchedule(CancellationToken cancellationToken, [FromBody] int id)
    {
        await _schedulerService.CreateSchedule(id);
    }

    [HttpPost("test")]
    public async Task Test(CancellationToken token, int id, int status)
    {
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