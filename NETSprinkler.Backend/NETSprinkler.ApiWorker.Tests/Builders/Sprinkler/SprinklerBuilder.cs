using NETSprinkler.Models.Entity.Status;
using NETSprinkler.Models.Entity.Valve;

namespace NETSprinkler.ApiWorker.Tests.Builders.Sprinkler;

public class SprinklerBuilder
{
    public static SprinklerValve CreateExistingSprinkler(string name = "Test", bool isEnabled = true, bool isOpen = false)
    {
        return new SprinklerValve()
        {
            Name = name,
            Status = new ValveStatus()
            {
                IsEnabled = isEnabled,
                IsOpen = isOpen
            }
        };
    }
}