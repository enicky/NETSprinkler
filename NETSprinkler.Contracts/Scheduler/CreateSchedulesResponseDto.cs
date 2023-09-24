namespace NETSprinkler.Contracts.Scheduler;

public class CreateSchedulesResponseDto
{
    public bool Success { get; set; }
    public int Id { get; set; }
    public string? Error { get; set; } = null;
}