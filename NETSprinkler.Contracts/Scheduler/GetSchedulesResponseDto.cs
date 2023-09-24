using NETSprinkler.Contracts.Entity.Schedule;

namespace NETSprinkler.Contracts.Scheduler;

public class GetSchedulesResponseDto
{
    public IEnumerable<ScheduleDto>? Schedules { get; set; } = null;
    public bool Success { get; set; } = true;
    public string? ErrorMessage { get; set; } = null;
}