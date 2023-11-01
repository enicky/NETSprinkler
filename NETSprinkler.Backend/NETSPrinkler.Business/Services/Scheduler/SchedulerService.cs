using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NETSprinkler.Common.Repositories;
using NETSprinkler.Common.Services;
using NETSprinkler.Contracts.Entity.Schedule;
using NETSprinkler.Models.Entity.Schedule;

namespace NETSprinkler.Business.Services.Scheduler;

public class SchedulerService: ServiceAsync<Schedule, ScheduleDto>, ISchedulerService
{
    private readonly ILogger<SchedulerService> _logger;
    private readonly IRepositoryAsync<Schedule> _schedulerRepository;
    private readonly IMapper _mapper;

    public SchedulerService(ILogger<SchedulerService> logger, IRepositoryAsync<Schedule> schedulerRepository, IMapper mapper)
        : base(schedulerRepository, mapper)
    {
        _logger = logger;
        _schedulerRepository = schedulerRepository;
        _mapper = mapper;
    }


    public async Task<List<ScheduleDto>> GetAll(CancellationToken cancellationToken)
    {
        _logger.LogInformation("[SchedulerService:GetAll] Retrieving all schedules");
        return _mapper.Map<List<ScheduleDto>>(await _schedulerRepository.Entities.ToListAsync(cancellationToken));
    }
}