using NETSprinkler.Models.Entity.Valve;

namespace NETSprinkler.ApiWorker.Business.Services.Valves;

public interface IValveService
{
    Task<SprinklerValve> GetSprinklerValveById(int retrievedJobSprinklerValveId);
    Task<bool> TurnOn(int sprinklerValveId);
    Task<bool> TurnOff(int sprinklerValveId);
}