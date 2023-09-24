using NETSprinkler.ApiWorker.Business.Exceptions;

namespace NETSprinkler.ApiWorker.Business.Drivers;

public class DummyDriver : IGpioDriver
{
    private readonly int _nrOfPins;
    public DummyPinValue[] Pins;

    public DummyDriver(int nrOfPins = 8)
    {
        _nrOfPins = nrOfPins;
        Pins = new DummyPinValue[_nrOfPins];
        InitializePins();
    }

    private void InitializePins()
    {
        for (var i = 0; i < _nrOfPins; i++)
        {
            Pins[i] = new DummyPinValue { PinNumber = i };
        }
    }

    public Task<bool> IsPinOpen(int pin)
    {
        if (pin > _nrOfPins)
            throw new InvalidPinSelectedException($"Pin with nr {pin} is not available (max pin {_nrOfPins})");
        return Task.FromResult(Pins[pin].IsOpen);
    }

    public Task OpenPin(int pin)
    {
        Pins[pin].Open();
        return Task.CompletedTask;
    }

    public Task ClosePin(int pin)
    {
        Pins    [pin].Close();
        return Task.CompletedTask;
    }
}

public class DummyPinValue
{
    public int PinNumber { get; set; }
    public bool IsOpen { get; set; } = false;

    public void Open()
    {
        IsOpen = true;
    }

    public void Close()
    {
        IsOpen = false;
    }
}