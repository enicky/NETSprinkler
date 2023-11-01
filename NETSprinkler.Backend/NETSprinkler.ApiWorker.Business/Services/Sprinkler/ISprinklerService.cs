namespace NETSprinkler.ApiWorker.Business.Services.Sprinkler;

public interface ISprinklerService
{
    Task StartAsync(int sprinklerValveId);
    Task StopAsync(int sprinklerValveId);
}