using Microsoft.Extensions.Logging;
using NETSprinkler.Business.Repositories;
using NETSprinkler.Contracts.Entity.Schedule;
using NETSprinkler.Contracts.Scheduler;
using NETSprinkler.Models.Entity.Schedule;

namespace NETSprinkler.Business.Scheduler;

public class SchedulerFacade: ISchedulerFacade
{
    private readonly ILogger<SchedulerFacade> _logger;
    private readonly IRepositoryAsync<Schedule> _schedulerRepository;

    public SchedulerFacade(ILogger<SchedulerFacade> logger, IRepositoryAsync<Schedule> schedulerRepository)
    {
        _logger = logger;
        _schedulerRepository = schedulerRepository;
    }
    public IEnumerable<ScheduleDto> GetAllSchedules()
    {
        _logger.LogInformation($"Retrieving all schedules that are available");
        var q =  _schedulerRepository.GetAll().ToList();
        return q;
    }
}