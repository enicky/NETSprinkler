using Microsoft.Extensions.Logging;
using NETSprinkler.ApiWorker.Business.MQTT;
using NETSprinkler.ApiWorker.Business.Services.Valves;
using NETSprinkler.Common.DbContext;
using NETSprinkler.Models.Entity.Valve;

namespace NETSprinkler.ApiWorker.Business.Services.Sprinkler;

public class SprinklerService: ISprinklerService
{
    private readonly ILogger<SprinklerService> _logger;
    private readonly IValveService _valveService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMqttService _mqttService;

    public SprinklerService(ILogger<SprinklerService> logger, IValveService valveService, IUnitOfWork unitOfWork, MqttClientServiceProvider mqttServiceProvider)
    {
        _logger = logger;
        _valveService = valveService;
        _unitOfWork = unitOfWork;
        this._mqttService = mqttServiceProvider.MqttService;
    }


    public async Task StartAsync(int sprinklerValveId)
    {
        _logger.LogInformation("[SprinklerService::StartAsync] Starting Sprinkler valve {SprinklerValveId} (if not running already)", sprinklerValveId);
        await _valveService.TurnOn(sprinklerValveId);
        await _mqttService.SendStatus(new SprinklerStatus
        {
            SprinklerId = sprinklerValveId,
            Status = SprinklerState.Open
        });
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task StopAsync(int sprinklerValveId)
    {
        _logger.LogInformation("[SprinklerService::StopAsync] Stopping Sprinkler valve {SprinklerValveId} (if not stopped already)",  sprinklerValveId);
        await _valveService.TurnOff(sprinklerValveId);
        await _mqttService.SendStatus(new SprinklerStatus
        {
            SprinklerId = sprinklerValveId,
            Status = SprinklerState.Closed
        });
        await _unitOfWork.SaveChangesAsync();
    }
}