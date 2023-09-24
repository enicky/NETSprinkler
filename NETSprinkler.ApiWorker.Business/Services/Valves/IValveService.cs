using NETSprinkler.Models.Entity.Valve;

namespace NETSprinkler.ApiWorker.Business.Services.Valves;

public interface IValveService
{
    Task<SprinklerValve> GetSprinklerValveById(int retrievedJobSprinklerValveId);
    Task TurnOn(int sprinklerValveId);
    Task TurnOff(int sprinklerValveId);
}