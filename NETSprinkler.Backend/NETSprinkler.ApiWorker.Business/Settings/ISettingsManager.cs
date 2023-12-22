using System;
using NETSprinkler.Contracts.Entity.Schedule;
using NETSprinkler.Contracts.Entity.Valve;

namespace NETSprinkler.ApiWorker.Business.Settings
{
    public interface ISettingsManager
    {
        Task<bool> DisableController(CancellationToken cancellationToken = default);
        Task<bool> EnableController(CancellationToken cancellationToken);
        List<SprinklerValveDto> GetAllValvesAsync();

        string GetMacAddressString();
        string GetUptimeValue();
        long LastRebootTime();
        long GetDeviceTime();

        List<ScheduleDto> GetAllSchedules(CancellationToken cancellationToken = default);


    }
}

