using NETSprinkler.Common.Services;
using NETSprinkler.Contracts.Entity.Valve;
using NETSprinkler.Models.Entity.Valve;

namespace NETSprinkler.ApiWorker.Business.Services.Valve;

public interface IValveService : IServiceAsync<SprinklerValve, SprinklerValveDto>
{
    Task<List<SprinklerValveDto>> GetAll(CancellationToken cancellationToken);
    Task<SprinklerValve> AddEmptyAndReturnValveIdAsync(SprinklerValveDto sprinklerValveDto);
    Task<bool> EnableValve(int valveId, bool enableValve);
    Task<List<SprinklerValveDto>> GetAllValvesWithSettings();
    Task<bool> Run(int valveId, int seconds);
}