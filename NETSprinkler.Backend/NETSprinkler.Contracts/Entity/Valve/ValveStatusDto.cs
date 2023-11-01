using AutoMapper;
using NETSprinkler.Contracts.Mapping;
using NETSprinkler.Models.Entity.Status;

namespace NETSprinkler.Contracts.Entity.Valve;

public class ValveStatusDto : EntityDto, IMapFrom
{
    public bool IsEnabled { get; set; } = true;
    public bool IsOpen { get; set; } = false;
    public int? SprinklerValveId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ValveStatusDto, ValveStatus>().ReverseMap();
    }
}