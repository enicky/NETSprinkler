using NETSprinkler.Contracts.Entity.Schedule;

namespace NETSprinkler.Contracts.Scheduler;

public class CreateSchedulesRequestDto
{
    public ScheduleDto Schedule { get; set; } = new();
}