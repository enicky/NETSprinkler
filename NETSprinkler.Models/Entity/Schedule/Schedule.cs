namespace NETSprinkler.Business.Entity.Schedule;

public class Schedule : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Cron { get; set; } = string.Empty;
}