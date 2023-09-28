using System.Device.Gpio;
using Microsoft.Extensions.Logging;
using NETSprinkler.Common.Extensions;

namespace NETSprinkler.ApiWorker.Business.Drivers;

public class RPiDriver: IGpioDriver
{
    private readonly ILogger<RPiDriver> _logger;
    //private readonly Pcf8574 _pcf8574;

    private byte _currentState = 0x0;

    private GpioPin pinLatch;
    private GpioPin pinData;
    private GpioPin pinClock;
    
    public RPiDriver(ILogger<RPiDriver> logger)
    {
        _logger = logger;
        /*
        var gpio = new GpioController();
        pinLatch = gpio.OpenPin(22); //RCLOCK
        pinLatch.SetPinMode(PinMode.Output);
        pinData = gpio.OpenPin(27);
        pinData.SetPinMode(PinMode.Output);
        pinClock = gpio.OpenPin(4);
        pinClock.SetPinMode(PinMode.Output);


        pinLatch.Write(PinValue.Low);
        pinClock.Write(PinValue.Low);
        pinData.Write(PinValue.Low);


        PulseRCLK();
        */
    }

    // Serial-In-Parallel-Out
    private void WriteSIPO(byte b)
    {

        for(var i = 0; i < 8; i++)
        {
            pinData.Write((b & (0x80 >> i)) > 0 ? PinValue.High : PinValue.Low);
            PulseRCLK();
        }
    }

    // pulse Register Clock / Latch Clock
    void PulseRCLK()
    {
        pinLatch.Write(PinValue.Low);
        pinLatch.Write(PinValue.High);
    }

    // Pulse Serial Clock
    void PulseSCLK()
    {
        pinClock.Write(PinValue.Low);
        pinClock.Write(PinValue.High);
    }

    /*
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
    }*/

    public Task OpenPin(int pin)
    {
        /* Span<PinValuePair> values = stackalloc PinValuePair[]
         {
             new PinValuePair(pin, PinValue.High)
         };
         _pcf8574.Write(values);
         return Task.CompletedTask;
        */

        
        _currentState = _currentState.SetBit(pin);



        int i = 0x01;
        
        i = i << pin;

        _logger.LogInformation($"[RPiDriver:OpenPin] Opening pin {pin} and i value {i} and HEX {Convert.ToString(_currentState, 2).PadLeft(8, '0')}");

        //WriteSIPO(_currentState);
        //WriteSIPO((byte)i);
        //PulseRCLK();

        return Task.CompletedTask;
    }

    public Task ClosePin(int pin)
    {

        _currentState = _currentState.UnsetBit(pin);
        int i = 0x01;
        i = i << pin;
        
        _logger.LogInformation($"[RPiDriver:ClosePin] Opening pin {pin} and i value {i} and HEX {Convert.ToString(_currentState, 2).PadLeft(8, '0')}");
        //WriteSIPO(_currentState);
        //WriteSIPO((byte)i);
        //PulseRCLK();

        return Task.CompletedTask;
    }
}