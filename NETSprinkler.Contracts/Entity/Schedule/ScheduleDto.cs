
using AutoMapper;
using NETSprinkler.Contracts.Mapping;

namespace NETSprinkler.Contracts.Entity.Schedule;

public class ScheduleDto : EntityDto, IMapFrom
{
    public string Name { get; set; } = string.Empty;
    public string Cron { get; set; } = string.Empty;
    public int StartHour { get; set; } = 0;
    public int StartMinute { get; set; } = 0;
    public int EndHour { get; set; } = 0;
    public int EndMinute { get; set; } = 0;
    public int? SprinklerValveId { get; set; }
    public IEnumerable<DayReferenceDto> DaysToRun { get; set; } = new List<DayReferenceDto>(); // starting with 0 = monday


    public void Mapping(Profile profile)
    {
        profile.CreateMap<ScheduleDto, Models.Entity.Schedule.Schedule>().ReverseMap();
    }
}