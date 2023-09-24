using NETSprinkler.Contracts.Entity.Schedule;
using NETSprinkler.Contracts.Scheduler;

namespace NETSprinkler.Business.Scheduler;

public interface ISchedulerFacade
{
    IEnumerable<ScheduleDto> GetAllSchedules();
}