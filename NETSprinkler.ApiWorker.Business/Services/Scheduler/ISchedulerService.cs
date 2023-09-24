using NETSprinkler.Models.Entity.Schedule;

namespace NETSprinkler.ApiWorker.Business.Services.Scheduler;

public interface ISchedulerService
{
    Task CreateSchedule(int id, CancellationToken token = default);
    Task<Schedule?> GetScheduleById(int id);
}