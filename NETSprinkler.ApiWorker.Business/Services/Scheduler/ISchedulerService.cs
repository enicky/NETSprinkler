using NETSprinkler.Models.Entity.Schedule;

namespace NETSprinkler.ApiWorker.Business.Services.Scheduler;

public interface ISchedulerService
{
    Task CreateSchedule(int id, CancellationToken token = default);
    Task DeleteSchedule(int id, CancellationToken cancellation = default);
    Task<Schedule?> GetScheduleById(int id);
}