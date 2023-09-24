namespace NETSprinkler.Contracts.Scheduler;

public abstract class ScheduleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Cron { get; set; } = string.Empty;
}