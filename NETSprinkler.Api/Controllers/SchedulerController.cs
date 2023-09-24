using Microsoft.AspNetCore.Mvc;
using NETSprinkler.Business.Services.Scheduler;
using NETSprinkler.Common.DbContext;
using NETSprinkler.Contracts.Scheduler;

namespace NETSprinkler.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchedulerController : Controller
{
    private readonly ILogger<SchedulerController> _logger;
    private readonly ISchedulerService _schedulerService;
    private readonly IUnitOfWork _unitOfWork;

    public SchedulerController(ILogger<SchedulerController> logger, 
        ISchedulerService schedulerService
        ,IUnitOfWork unitOfWork
        )
    {
        _logger = logger;
        _schedulerService = schedulerService;
        _unitOfWork = unitOfWork;
    }

    // GET
    [HttpGet("GetSchedules", Name = "GetSchedules")]
    public async Task<GetSchedulesResponseDto> GetSchedules(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieve all schedules");
        
        try
        {
            var allSchedules = await _schedulerService.GetAll(cancellationToken);
            return new GetSchedulesResponseDto
            {
                Success = true,
                Schedules = allSchedules
            };
        }
        catch (Exception ex)
        {
            return new GetSchedulesResponseDto
            {
                ErrorMessage = ex.Message,
                Success = false
            };
        }

        
    }

    [HttpPost("CreateSchedule")]
    public async Task<CreateSchedulesResponseDto> CreateSchedule(CancellationToken cancellationToken ,  
        CreateSchedulesRequestDto request)
    {
        await _schedulerService.AddAsync(request.Schedule);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new CreateSchedulesResponseDto()
        {
            Success = true,
            Error = null
        };
    }

    [HttpDelete("DeleteSchedule")]
    public async Task<DeleteScheduleResultDto> DeleteSchedule(CancellationToken cancellationToken,
        int id)
    {
        await _schedulerService.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new DeleteScheduleResultDto
        {
            Success = true
        };
    }
}