namespace NETSprinkler.Models.Entity.Status;

public class SprinklerStatus: Entity
{
    public int? SprinklerId { get; set; }
    public bool IsEnabled { get; set; } = true;
    public bool IsOpen { get; set; } = false;
}