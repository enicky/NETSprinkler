using NETSprinkler.Common.Services;
using NETSprinkler.Contracts.Entity.Valve;
using NETSprinkler.Models.Entity.Valve;

namespace NETSprinkler.Business.Services.Scheduler;

public interface IValveService: IServiceAsync<SprinklerValve, SprinklerValveDto>
{
    Task<List<SprinklerValveDto>> GetAll(CancellationToken cancellationToken);
    Task AddEmptyAsync(SprinklerValveDto sprinklerValveDto);
}