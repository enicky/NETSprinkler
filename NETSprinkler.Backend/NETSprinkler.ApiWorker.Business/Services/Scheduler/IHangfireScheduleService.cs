using NETSprinkler.Models.Entity.Schedule;

namespace NETSprinkler.ApiWorker.Business.Services.Scheduler;

public interface IHangfireScheduleService
{
    Task CreateSchedule(int id, CancellationToken token = default);
    Task DeleteSchedule(int id, CancellationToken cancellation = default);
    Task<List<string>> GetAllHangfireSchedules(CancellationToken cancellationToken);
    Task<Schedule?> GetScheduleById(int id);
    Task<bool> RunManually(int valveId, int seconds);
}