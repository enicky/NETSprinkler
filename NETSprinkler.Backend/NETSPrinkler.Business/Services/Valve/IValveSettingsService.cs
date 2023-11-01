using NETSprinkler.Common.Services;
using NETSprinkler.Contracts.Entity.Valve;
using NETSprinkler.Models.Entity.Status;

namespace NETSprinkler.Business.Services.Valve;

public interface IValveSettingsService: IServiceAsync<ValveStatus, ValveStatusDto>
{
    Task CreateEmptySettingForValve(int createdSprinklerId);
}