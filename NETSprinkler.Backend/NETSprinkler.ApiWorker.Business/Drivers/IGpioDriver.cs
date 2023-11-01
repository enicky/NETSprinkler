namespace NETSprinkler.ApiWorker.Business.Drivers;

public interface IGpioDriver
{
    //Task<bool> IsPinOpen(int pin);
    Task OpenPin(int pin);
    Task ClosePin(int pin);
}