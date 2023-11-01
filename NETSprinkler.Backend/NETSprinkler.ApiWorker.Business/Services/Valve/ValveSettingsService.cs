using AutoMapper;
using Microsoft.Extensions.Logging;
using NETSprinkler.Common.Repositories;
using NETSprinkler.Common.Services;
using NETSprinkler.Contracts.Entity.Valve;
using NETSprinkler.Models.Entity.Status;

namespace NETSprinkler.ApiWorker.Business.Services.Valve;

public class ValveSettingsService : ServiceAsync<ValveStatus, ValveStatusDto>, IValveSettingsService
{
    private readonly ILogger<ValveSettingsService> _logger;

    public ValveSettingsService(ILogger<ValveSettingsService> logger,
        IRepositoryAsync<ValveStatus> repositoryAsync, IMapper mapper) : base(repositoryAsync, mapper)
    {
        _logger = logger;
    }

    public Task CreateEmptySettingForValve(int createdSprinklerId)
    {
        _logger.LogDebug("[ValveSettingsService::CreateEmptySettingForValve] creating an empty settings record for id {CreatedSprinklerId}", createdSprinklerId);
        return AddAsync(new ValveStatusDto()
        {
            IsEnabled = true,
            IsOpen = false,
            SprinklerValveId = createdSprinklerId
        });
    }
}