using NETSprinkler.Models.Entity.Status;

namespace NETSprinkler.Models.Entity.Valve;

public class SprinklerValve : Entity
{
    public string Name { get; set; } = string.Empty;

    public int Port { get; set; } = 0;

    public ValveStatus? Status { get; set; } = new() { IsEnabled = true, IsOpen = false };
}