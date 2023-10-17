using Microsoft.AspNetCore.Mvc;
using NETSprinkler.Business.Services.Valve;
using NETSprinkler.Common.DbContext;
using NETSprinkler.Contracts.Entity.Valve;

namespace NETSprinkler.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ValveController: Controller
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

    [HttpPost("CreateValve")]
    public async Task<CreateValveResponseDto> CreateValve(CancellationToken token, CreateValveRequestDto req)
    {
        var createdSprinkler = await _valveService.AddEmptyAndReturnValveIdAsync(new SprinklerValveDto { Name = req.Name, });
        await _unitOfWork.SaveChangesAsync(token);
        return new CreateValveResponseDto()
        {
            Success = true,
            ValveId = createdSprinkler.Id
        };
    }

    [HttpGet("GetAllValves")]
    public List<SprinklerValveDto> GetAllValves(CancellationToken token)
    {
        var allValves = _valveService.GetAll();
        return allValves.ToList();
    }

    [HttpDelete("DeleteValve")]
    public async Task<DeleteValveResponseDto> DeleteValve(CancellationToken token, int id)
    {
        await _valveService.DeleteAsync(id);
        return new DeleteValveResponseDto { Success = true };
    }
}