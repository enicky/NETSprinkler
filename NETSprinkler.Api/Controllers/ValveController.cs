using Microsoft.AspNetCore.Mvc;
using NETSprinkler.Business.Services.Valve;
using NETSprinkler.Common.DbContext;
using NETSprinkler.Contracts.Entity.Valve;

namespace NETSprinkler.Controllers;

[ApiController]
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
}