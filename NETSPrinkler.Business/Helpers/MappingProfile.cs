using System.Reflection;
using AutoMapper;
using NETSprinkler.Contracts.Entity.Schedule;
using NETSprinkler.Helpers.Mapping;
using NETSprinkler.Models.Entity.Schedule;

namespace NETSprinkler.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void ApplyMappingsFromAssembly(Assembly a)
    {
        var types = typeof(ScheduleDto).Assembly.GetExportedTypes()
            .Where(x => typeof(IMapFrom).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .ToList();
        foreach (var t in types)
        {
            var instance = Activator.CreateInstance(t);
            var mi = t.GetMethod("Mapping");
            mi?.Invoke(instance, new object?[]{this});
        }

    }
}