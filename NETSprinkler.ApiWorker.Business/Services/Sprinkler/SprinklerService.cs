using Microsoft.Extensions.Logging;
using NETSprinkler.ApiWorker.Business.Services.Valves;
using NETSprinkler.Common.DbContext;

namespace NETSprinkler.ApiWorker.Business.Services.Sprinkler;

public class SprinklerService: ISprinklerService
{
    private readonly ILogger<SprinklerService> _logger;
    private readonly IValveService _valveService;
    private readonly IUnitOfWork _unitOfWork;

    public SprinklerService(ILogger<SprinklerService> logger, IValveService valveService, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _valveService = valveService;
        _unitOfWork = unitOfWork;
    }


    public async Task StartAsync(int sprinklerValveId)
    {
        _logger.LogInformation("[SprinklerService::StartAsync] Starting Sprinkler valve {SprinklerValveId} (if not running already)", sprinklerValveId);
        await _valveService.TurnOn(sprinklerValveId);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task StopAsync(int sprinklerValveId)
    {
        _logger.LogInformation("[SprinklerService::StopAsync] Stopping Sprinkler valve {SprinklerValveId} (if not stopped already)",  sprinklerValveId);
        await _valveService.TurnOff(sprinklerValveId);
        await _unitOfWork.SaveChangesAsync();
    }
}