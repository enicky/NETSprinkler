using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NETSprinkler.Common.Repositories;
using NETSprinkler.Common.Services;
using NETSprinkler.Contracts.Entity.Valve;
using NETSprinkler.Models.Entity.Valve;

namespace NETSprinkler.Business.Services.Scheduler;

public class ValveService : ServiceAsync<SprinklerValve, SprinklerValveDto>, IValveService
{
    private readonly ILogger<ValveService> _logger;
    private readonly IMapper _mapper;
    private readonly IRepositoryAsync<SprinklerValve> _repository;

    public ValveService(ILogger<ValveService> logger, IMapper mapper, IRepositoryAsync<SprinklerValve> repository)
        : base(repository, mapper)
    {
        _logger = logger;
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<List<SprinklerValveDto>> GetAll(CancellationToken cancellationToken)
    {
        _logger.LogDebug($"[ValveService::GetAll] retrieving all Valves stored in DB");
        return _mapper.Map<List<SprinklerValveDto>>(await _repository.Entities.ToListAsync(cancellationToken));
    }

    public Task AddEmptyAsync(SprinklerValveDto sprinklerValveDto)
    {
        return AddAsync(sprinklerValveDto);
    }
}