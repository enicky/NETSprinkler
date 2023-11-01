namespace NETSprinkler.Contracts.Entity.Valve;

public class CreateValveRequestDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Port { get; set; } = 0;
}