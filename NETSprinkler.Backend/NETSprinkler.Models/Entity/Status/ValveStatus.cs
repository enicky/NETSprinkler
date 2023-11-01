using NETSprinkler.Models.Entity.Valve;

namespace NETSprinkler.Models.Entity.Status;

public class ValveStatus: Entity
{
    public bool IsEnabled { get; set; } = true;
    public bool IsOpen { get; set; } = false;

    
    public int? SprinklerValveId { get; set; }
    public SprinklerValve SprinklerValve { get; set; } = null!;
}