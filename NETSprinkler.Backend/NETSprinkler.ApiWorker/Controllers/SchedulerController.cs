using Microsoft.AspNetCore.Mvc;
using NETSprinkler.ApiWorker.Business.Services.Scheduler;
using NETSprinkler.Common.DbContext;
using NETSprinkler.Contracts.Scheduler;

namespace NETSprinkler.ApiWorker.Controllers
{
    [ApiController]
	[Route("api/[controller]")]
	public class SchedulerController
	{
        private readonly ILogger<SchedulerController> logger;
        private readonly IScheduleService schedulerService;
        private readonly IUnitOfWork unitOfWork;

        public SchedulerController(ILogger<SchedulerController> logger,
			IScheduleService schedulerService,
            IUnitOfWork unitOfWork)
		{
            this.logger = logger;
            this.schedulerService = schedulerService;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("GetSchedules", Name = "GetSchedules")]
        public async Task<GetSchedulesResponseDto> GetSchedules(CancellationToken cancellationToken)
        {
            logger.LogInformation("Retrieve all schedules");

            try
            {
                var allSchedules = await schedulerService.GetAll(cancellationToken);
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
        public async Task<CreateSchedulesResponseDto> CreateSchedule(CancellationToken cancellationToken,
       CreateSchedulesRequestDto request)
        {
            await schedulerService.AddAsync(request.Schedule);
            await unitOfWork.SaveChangesAsync(cancellationToken);
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
            await schedulerService.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return new DeleteScheduleResultDto
            {
                Success = true
            };
        }
    }
}

