using NETSprinkler.Models.Entity.Valve;

namespace NETSprinkler.Models.Entity.Schedule;

public class Schedule : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Cron { get; set; } = string.Empty;
    
    public int StartHour { get; set; } = 0;
    public int StartMinute { get; set; } = 0;
    public int EndHour { get; set; } = 0;
    public int EndMinute { get; set; } = 0;
    
    public int? SprinklerValveId { get; set; }
    public SprinklerValve? Sprinkler { get; set; } = null;
    
    public IEnumerable<DayReference> DaysToRun { get; set; } = new List<DayReference>(); // starting with 0 = monday
}