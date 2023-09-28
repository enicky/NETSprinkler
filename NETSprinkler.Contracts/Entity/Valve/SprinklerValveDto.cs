using AutoMapper;
using NETSprinkler.Contracts.Mapping;
using NETSprinkler.Models.Entity.Valve;

namespace NETSprinkler.Contracts.Entity.Valve;

public class SprinklerValveDto : EntityDto, IMapFrom
{
    public string Name { get; set; } = string.Empty;
    public int Port { get; set; } = 0;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<SprinklerValveDto, SprinklerValve>().ReverseMap();
    }
}