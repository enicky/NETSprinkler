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

    public Task<SprinklerValve> GetSprinklerValveById(int sprinklerValveId)
    {
        _logger.LogDebug("[ValveService::GetSprinklerValveById] Getting valve with id {SprinklerValveId}", sprinklerValveId);
        var r = GetById(sprinklerValveId);
        return r!;
    }

    public async Task TurnOn(int sprinklerValveId)
    {
        var w =  await GetByIdWithInclude(sprinklerValveId, valve => valve.Status!);
        if (w == null)
        {
            _logger.LogError($"[ValveService:TurnOn] There was an error retrieving info about valve with id {sprinklerValveId}");
            throw new SprinklerNotFoundException($"Sprinkler with id {sprinklerValveId} not found");
        }
        // Perform GPIO functionality
        await _gpioDriver.OpenPin(w.Port);
        w!.Status!.IsOpen = true;
        
    }

    public async Task TurnOff(int sprinklerValveId)
    {
        var w = await GetByIdWithInclude(sprinklerValveId, valve => valve.Status!);
        if (w == null)
        {
            _logger.LogError($"[ValveService:TurnOff] There was an error retrieving info about valve with id {sprinklerValveId}");
            throw new SprinklerNotFoundException($"Sprinkler with id {sprinklerValveId} not found");
        }
        // Perform GPIO Functionality
        await _gpioDriver.ClosePin(w.Port);
        w!.Status!.IsOpen = false;
    }
}