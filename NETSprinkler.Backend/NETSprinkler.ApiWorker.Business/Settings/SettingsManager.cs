using System.Net.NetworkInformation;
using Microsoft.Extensions.Logging;
using NETSprinkler.ApiWorker.Business.Services.Scheduler;
using NETSprinkler.ApiWorker.Business.Services.Valve;
using NETSprinkler.Contracts.Entity.Valve;

namespace NETSprinkler.ApiWorker.Business.Settings
{
    public class SettingsManager : ISettingsManager
	{
        private readonly ILogger<SettingsManager> logger;
        private readonly IValveService valveService;
        private readonly Services.Valves.IValveService valveSwitchService;
        private readonly IScheduleService scheduleService;
        private readonly IHangfireScheduleService hangfireScheduleService;

        public SettingsManager(ILogger<SettingsManager> logger, IValveService valveService, Services.Valves.IValveService valveSwitchService,
            IScheduleService scheduleService, IHangfireScheduleService hangfireScheduleService)
		{
            this.logger = logger;
            this.valveService = valveService;
            this.valveSwitchService = valveSwitchService;
            this.scheduleService = scheduleService;
            this.hangfireScheduleService = hangfireScheduleService;
        }

        public async Task<bool> DisableController(CancellationToken cancellationToken = default)
        {
            try
            {
                await StopAndDisableAllValves();
                await StopAndDisableAllSchedules(cancellationToken);
                await StopAndDisableAllHangfireSchedules(cancellationToken);
            }catch(Exception ex)
            {
                logger.LogError(ex, $"[SettingsManager:DisableController] There was an error disabling the controller");
                return false;
            }
            return true;
        }

        public async Task<bool> EnableController(CancellationToken cancellationToken)
        {
            try
            {
                await EnableAllValves(cancellationToken);
                await EnableAllSchedules(cancellationToken);
                await EnableAllHangfireSchedules(cancellationToken);

            }catch(Exception ex)
            {
                logger.LogError(ex, $"[SettingsManager:DisableController] There was an error ENABLING the controller");
                return false;
            }
            return true;
        }

        private async Task EnableAllValves(CancellationToken cancellationToken)
        {
            var allValves = await valveService.GetAll(cancellationToken).ConfigureAwait(false);
            foreach(var x in allValves)
            {
                x.Enabled = true;
            }
        }

        private async Task EnableAllSchedules(CancellationToken cancellationToken)
        {
            var allSchedules = await scheduleService.GetAll(cancellationToken).ConfigureAwait(false);
            allSchedules.ForEach(async q => await scheduleService.EnableSchedule(q.Id));
        }

        private async Task EnableAllHangfireSchedules(CancellationToken cancellationToken)
        {
            var allSchedules = await scheduleService.GetAll(cancellationToken).ConfigureAwait(false);
            allSchedules.ForEach(async q => await hangfireScheduleService.CreateSchedule(q.Id, cancellationToken));
        }


        private async Task StopAndDisableAllHangfireSchedules(CancellationToken cancellationToken)
        {
            var allSchedules = await scheduleService.GetAll(cancellationToken).ConfigureAwait(false);
            foreach(var x in allSchedules)
            {
                await hangfireScheduleService.DeleteSchedule(x.Id, cancellationToken);
            }
        }

        private async Task StopAndDisableAllSchedules(CancellationToken cancellationToken)
        {
            var allSchedules = await scheduleService.GetAll(cancellationToken).ConfigureAwait(false);
            foreach(var x in allSchedules)
            {
                await scheduleService.DisableSchedule(x.Id);
            }
        }

        public List<SprinklerValveDto> GetAllValvesAsync()
        {
            var allValves = valveService.GetAll();
            if(allValves is null)
            {
                throw new NullReferenceException("allValves may not be null !! ");
            }
            return allValves.ToList();
        }

        public string GetMacAddressString()
        {
            return NetworkInterface
                   .GetAllNetworkInterfaces().Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                   .Select(nic => nic.GetPhysicalAddress().ToString()).FirstOrDefault();
        }

        public string GetUptimeValue()
        {
            TimeSpan uptime = TimeSpan.FromMilliseconds(Environment.TickCount);
            return uptime.ToString("c");
        }
        public long LastRebootTime()
        {
            var uptime = TimeSpan.FromMilliseconds(Environment.TickCount);
            var startDate = DateTime.Now.Subtract(  uptime);
            
            return startDate.Ticks;
        }

        private async Task StopAndDisableAllValves()
        {
            var allValves = valveService.GetAll();
            foreach(var v in allValves)
            {
                await valveSwitchService.TurnOff(v.Id);
                v.Enabled = false;
            }
        }

       
    }
}

