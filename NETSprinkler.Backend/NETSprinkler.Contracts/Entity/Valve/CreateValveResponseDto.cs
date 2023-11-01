using NETSprinkler.Contracts.Scheduler;

namespace NETSprinkler.Contracts.Entity.Valve;

public class CreateValveResponseDto: BaseResponseDto
{
    public int ValveId { get; set; }
}