using Microsoft.Extensions.Logging;
using NETSprinkler.ApiWorker.Business.Drivers;
using NETSprinkler.Common.Exceptions.Sprinkler;
using NETSprinkler.Common.Repositories;
using NETSprinkler.Common.Services;
using NETSprinkler.Models.Entity.Valve;

namespace NETSprinkler.ApiWorker.Business.Services.Valves;

public class ValveService: ServiceAsync<SprinklerValve>, IValveService
{
    private readonly ILogger<ValveService> _logger;
    private readonly IGpioDriver _gpioDriver;

    public ValveService(ILogger<ValveService> logger, 
        IRepositoryAsync<SprinklerValve> repositoryAsync,
        IGpioDriver gpioDriver) : base(repositoryAsync)
    {
        _logger = logger;
        _gpioDriver = gpioDriver;
    }

    public List<SprinklerValve> GetAllValvesWithSettings()
    {
        List<SprinklerValve> sprinklerValves = GetAllWithInclude(x => x.Status).ToList();
        return sprinklerValves;
    }

    public Task<SprinklerValve> GetSprinklerValveById(int sprinklerValveId)
    {
        _logger.LogDebug("[ValveService::GetSprinklerValveById] Getting valve with id {SprinklerValveId}", sprinklerValveId);
        var r = GetById(sprinklerValveId);
        return r!;
    }

    public async Task<bool> TurnOn(int sprinklerValveId)
    {
        var w =  await GetByIdWithInclude(sprinklerValveId, valve => valve.Status!);
        if (w == null || w.Status == null)
        {
            _logger.LogError($"[ValveService:TurnOn] There was an error retrieving info about valve with id {sprinklerValveId}");
            throw new SprinklerNotFoundException($"Sprinkler with id {sprinklerValveId} not found");
        }
        if(w.Status.IsOpen)
        {
            _logger.LogInformation($"[ValveService:TurnOn] Valve was already open. Do not trigger GPIO driver again");
            return false;
        }
        // Perform GPIO functionality
        await _gpioDriver.OpenPin(w.Port);
        w!.Status!.IsOpen = true;
        return true;
    }

    public async Task<bool> TurnOff(int sprinklerValveId)
    {
        var w = await GetByIdWithInclude(sprinklerValveId, valve => valve.Status!);
        if (w == null || w.Status == null)
        {
            _logger.LogError($"[ValveService:TurnOff] There was an error retrieving info about valve with id {sprinklerValveId}");
            throw new SprinklerNotFoundException($"Sprinkler with id {sprinklerValveId} not found");
        }
        if (!w.Status.IsOpen)
        {
            _logger.LogInformation($"[ValveService:TurnOff] Valve was already closed. Do not trigger GPIO driver again");
            return false;
        }
        // Perform GPIO Functionality
        await _gpioDriver.ClosePin(w.Port);
        w!.Status!.IsOpen = false;
        return true;
    }
}