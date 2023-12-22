using NETSprinkler.Contracts.Entity.Schedule;
using NETSprinkler.Contracts.Entity.Valve;

namespace NETSprinkler.Contracts.Settings
{
    public class AllSettings
	{

		public string FirmwareVersion { get; set; } = string.Empty;
		public string FirmwareMinorVersion { get; set; } = string.Empty;
        public string LastRebootCause { get; set; } = string.Empty;
        public string LastRebootCauseName { get; set; } = string.Empty;
        public string MacAddress { get; set; } = string.Empty;
        public string UpTime { get; set; } = string.Empty;
        public List<SprinklerValveDto> Valves { get; set; } = new List<SprinklerValveDto>();
        public long LastRebootTime { get; set; }
        public long DeviceTime { get; set; }
        public List<ScheduleDto> Schedules { get; set; } = new List<ScheduleDto>();
    }
}

