using NETSprinkler.Common.Services;
using NETSprinkler.Contracts.Entity.Schedule;
using NETSprinkler.Models.Entity.Schedule;

namespace NETSprinkler.Business.Services.Scheduler;

public interface ISchedulerService: IServiceAsync<Schedule, ScheduleDto>
{
    Task<List<ScheduleDto>> GetAll(CancellationToken cancellationToken);
}