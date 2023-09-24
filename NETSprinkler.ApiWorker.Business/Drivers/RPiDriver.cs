using System.Device.Gpio;
using System.Device.I2c;
using Iot.Device.Pcx857x;
using Microsoft.Extensions.Logging;

namespace NETSprinkler.ApiWorker.Business.Drivers;

public class RPiDriver: IGpioDriver
{
    private readonly ILogger<RPiDriver> _logger;
    private readonly Pcf8574 _pcf8574;
    
    public RPiDriver(ILogger<RPiDriver> logger)
    {
        _logger = logger;
        var i2CSettings = new I2cConnectionSettings(1, 22);
        var i2CDevice = I2cDevice.Create(i2CSettings);
        _pcf8574 = new Pcf8574(i2CDevice);

        ReadPinValue(1);
    }

    private PinValue ReadPinValue(int pin)
    {
        Span<PinValuePair> values = stackalloc PinValuePair[]
        {
            new PinValuePair(pin, default)
        };
        _pcf8574.Read(values);
        return values[0].PinValue;
    }

    public Task<bool> IsPinOpen(int pin)
    {
        _logger.LogDebug("[RPiDriver::IsPinOpen] Reading value from pin {Pin}", pin);
        var pinValue = ReadPinValue(pin);
        _logger.LogDebug("[RPiDriver::IsPinOpen] is pin open ? {IsOpen}", pinValue == PinValue.High);
        return Task.FromResult(pinValue == PinValue.High);
    }

    public Task OpenPin(int pin)
    {
        Span<PinValuePair> values = stackalloc PinValuePair[]
        {
            new PinValuePair(pin, PinValue.High)
        };
        _pcf8574.Write(values);
        return Task.CompletedTask;
    }

    public Task ClosePin(int pin)
    {
        Span<PinValuePair> values = stackalloc PinValuePair[]
        {
            new PinValuePair(pin, PinValue.Low)
        };
        _pcf8574.Write(values);
        return Task.CompletedTask;
    }
}