using Microsoft.AspNetCore.Mvc;
using NETSprinkler.ApiWorker.Business.Services.Valve;
using NETSprinkler.Common.DbContext;
using NETSprinkler.Contracts.Entity.Valve;

namespace NETSprinkler.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ValveController : Controller
{
    private readonly ILogger<ValveController> _logger;
    private readonly IValveService _valveService;
    private readonly IValveSettingsService _valveSettingsService;
    private readonly IUnitOfWork _unitOfWork;

    public ValveController(ILogger<ValveController> logger,
        IValveService valveService,
        IValveSettingsService valveSettingsService,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _valveService = valveService;
        _valveSettingsService = valveSettingsService;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("GetAllValves")]
    public List<SprinklerValveDto> GetAllValves(CancellationToken token)
    {
        _logger.LogInformation($"[ValveController:GetAllValve] retrieving all valves");
        var allValves = _valveService.GetAll();
        return allValves.ToList();
    }

    [HttpPost("CreateValve")]
    public async Task<CreateValveResponseDto> CreateValve(CancellationToken token, CreateValveRequestDto req)
    {
        _logger.LogInformation($"[ValveController:CreateValve] Creating valve");

        var createdSprinkler = await _valveService.AddEmptyAndReturnValveIdAsync(new SprinklerValveDto { Name = req.Name, });
        await _unitOfWork.SaveChangesAsync(token);
        return new CreateValveResponseDto()
        {
            Success = true,
            ValveId = createdSprinkler.Id
        };
    }

    [HttpDelete("DeleteValve")]
    public async Task<DeleteValveResponseDto> DeleteValve(CancellationToken token, int id)
    {
        _logger.LogInformation($"[ValveController:DeleteValve] Deleting valve by id {id}");
        await _valveService.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync(token);
        return new DeleteValveResponseDto { Success = true };
    }

    [HttpPost("EnableValve")]
    public async Task<EnableValveReponseDto> EnableValve(CancellationToken token, [FromBody] EnableValveRequestDto req)
    {
        _logger.LogInformation($"[ValveController:EnableValve] Set valve {req.ValveId} to status {req.EnableValve}");

        var result = await _valveService.EnableValve(req.ValveId, req.EnableValve);
        await _unitOfWork.SaveChangesAsync(token);
        return new EnableValveReponseDto { Success = result };
    }

    [HttpPost("Run")]
    public async Task<RunValveResponseDto> Run(CancellationToken token, [FromBody] RunValveRequestDto req)
    {
        _logger.LogDebug($"[ValveController:Run] Start running valve {req.ValveId} for {req.Seconds} seconds");
        var result = await _valveService.Run(req.ValveId, req.Seconds);
        await _unitOfWork.SaveChangesAsync(token);
        return new RunValveResponseDto { Success = result };

    }
}