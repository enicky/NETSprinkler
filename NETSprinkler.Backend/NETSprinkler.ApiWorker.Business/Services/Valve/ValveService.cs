using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NETSprinkler.ApiWorker.Business.Services.Scheduler;
using NETSprinkler.Common.Repositories;
using NETSprinkler.Common.Services;
using NETSprinkler.Contracts.Entity.Valve;
using NETSprinkler.Models.Entity.Valve;

namespace NETSprinkler.ApiWorker.Business.Services.Valve;

public class ValveService : ServiceAsync<SprinklerValve, SprinklerValveDto>, IValveService
{
    private readonly ILogger<ValveService> _logger;
    private readonly IMapper _mapper;
    private readonly IRepositoryAsync<SprinklerValve> _repository;

    private readonly IScheduleService _scheduleService;
    private readonly IHangfireScheduleService _hangfireScheduleService;

    public ValveService(ILogger<ValveService> logger, IMapper mapper, IRepositoryAsync<SprinklerValve> repository,
        IScheduleService scheduleService, IHangfireScheduleService hangfireScheduleService)
        : base(repository, mapper)
    {
        _logger = logger;
        _mapper = mapper;
        _repository = repository;
        _scheduleService = scheduleService;
        _hangfireScheduleService = hangfireScheduleService;

    }

    public async Task<List<SprinklerValveDto>> GetAll(CancellationToken cancellationToken)
    {
        _logger.LogDebug($"[ValveService::GetAll] retrieving all Valves stored in DB");
        return _mapper.Map<List<SprinklerValveDto>>(await _repository.Entities.ToListAsync(cancellationToken));
    }

    public async Task<SprinklerValve> AddEmptyAndReturnValveIdAsync(SprinklerValveDto sprinklerValveDto)
    {
        var saved = await AddAsync(sprinklerValveDto);
        return saved;

    }

    public async Task<bool> EnableValve(int valveId, bool enableValve)
    {
        var valve = await _repository.GetById(valveId) ?? throw new Exception("Valve not found");
        valve.Enabled = enableValve;
        return true;
    }

    public async Task<List<SprinklerValveDto>> GetAllValvesWithSettings()
    {
        var allValves = await _repository.GetAllWithInclude(q => q.Status).ToListAsync();
        var mappedValves = _mapper.Map<List<SprinklerValveDto>>(allValves);
        return mappedValves;
    }

    public Task<bool> Run(int valveId, int seconds)
    {
        return _hangfireScheduleService.RunManually(valveId, seconds);
    }
}