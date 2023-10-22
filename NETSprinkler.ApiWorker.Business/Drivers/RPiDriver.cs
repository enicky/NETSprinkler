using System.Device.Gpio;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NETSprinkler.Common.Config.Gpio;
using NETSprinkler.Common.Extensions;

namespace NETSprinkler.ApiWorker.Business.Drivers;

public class RPiDriver : IGpioDriver
{
    private readonly ILogger<RPiDriver> _logger;
    private readonly GpioConfigurationOptions _options;

    //private readonly Pcf8574 _pcf8574;

    private byte _currentState = 0x0;

    private readonly GpioPin pinLatch;
    private readonly GpioPin pinData;
    private readonly GpioPin pinClock;

    public RPiDriver(ILogger<RPiDriver> logger, IOptions<GpioConfigurationOptions> options)
    {
        _logger = logger;
        _options = options.Value;
        var gpio = new GpioController();
        var nbrScheme = gpio.NumberingScheme;
        _logger.LogInformation($"[] numberingschema {nbrScheme}");

        if (_options.Enabled)
        {
            pinLatch = gpio.OpenPin(22, PinMode.Output); //RCLOCK
            pinData = gpio.OpenPin(27, PinMode.Output);
            pinClock = gpio.OpenPin(4, PinMode.Output);

            pinLatch.Write(PinValue.Low);
            pinClock.Write(PinValue.Low);
            pinData.Write(PinValue.Low);


            PulseLatch();
        }
    }

    // Serial-In-Parallel-Out
    private void WriteSIPO(byte b)
    {
        _logger.LogInformation($"[WriteSIPO] Start Looping over byte {b}");
        for (var i = 0; i < 8; i++)
        {
            var pinValue = (b & (0x80 >> i)) > 0 ? PinValue.High : PinValue.Low;
            _logger.LogInformation($"[WriteSIPO] i {i} => {pinValue} ==> {(b & (0x80 >> i))}");
            pinClock.Write(PinValue.Low);
            pinData.Write(pinValue);
            pinClock.Write(PinValue.High);
            //pinClock.Write(PinValue.Low);
            //pinClock.Write(PinValue.High);
            //PulseSerialClock();
        }
    }

    // pulse Register Clock / Latch Clock
    void PulseLatch()
    {
        _logger.LogInformation("[] Start PulseLatch");
        pinLatch.Write(PinValue.High);
        pinLatch.Write(PinValue.Low);
        _logger.LogInformation("[] Finished PulseLatch");
    }

    // Pulse Serial Clock
    void PulseSerialClock()
    {
        _logger.LogInformation("[] Start Pulse Serial Clock");
        pinClock.Write(PinValue.High);
        pinClock.Write(PinValue.Low);
        _logger.LogInformation("[] Finished Pulse Serial Clock");
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

        _logger.LogInformation($"[OpenPin] Start opening on pin {pin}");
        _currentState = _currentState.SetBit(pin);


        int i = 0x01;

        i = i << pin;
        _logger.LogInformation($"[RPiDriver:OpenPin] Opening pin {pin} and i value {Convert.ToString(i, 2).PadLeft(8, '0')} and HEX {Convert.ToString(_currentState, 2).PadLeft(8, '0')}"); _logger.LogInformation("[OpenPin] Start pulling Latch low");
        if (!_options.Enabled) return Task.CompletedTask;

        if (_options.Enabled)
        {
            pinClock.SetPinMode(PinMode.Output);
            pinData.SetPinMode(PinMode.Output);
            pinLatch.SetPinMode(PinMode.Output);

            //pinLatch.Write(PinValue.Low);
            //_logger.LogInformation("[OpenPin] Finished pulling latch low");
            pinClock.Write(PinValue.Low);
            pinLatch.Write(PinValue.Low);
            pinClock.Write(PinValue.High);

            WriteSIPO(_currentState);

            pinClock.Write(PinValue.Low);
            pinLatch.Write(PinValue.High);
            pinClock.Write(PinValue.High);


            //_logger.LogInformation("[OpenPin] Start pulling Latch High");
            //        pinLatch.Write(PinValue.High);
            //_logger.LogInformation("[OpenPin] Finished pulling Latch High");

            pinClock.SetPinMode(PinMode.Input);
            pinData.SetPinMode(PinMode.Input);
            pinLatch.SetPinMode(PinMode.Input);
        }
        return Task.CompletedTask;
    }

    public Task ClosePin(int pin)
    {
        _currentState = _currentState.UnsetBit(pin);
        int i = 0x01;
        i = i << pin;

        _logger.LogInformation($"[RPiDriver:ClosePin] Opening pin {pin} and i value {Convert.ToString(i, 2).PadLeft(8, '0')} and HEX {Convert.ToString(_currentState, 2).PadLeft(8, '0')}");
        if (!_options.Enabled) return Task.CompletedTask;

        if (_options.Enabled)
        {
            pinClock.SetPinMode(PinMode.Output);
            pinData.SetPinMode(PinMode.Output);
            pinLatch.SetPinMode(PinMode.Output);

            WriteSIPO(_currentState);
            PulseLatch();

            pinClock.SetPinMode(PinMode.Input);
            pinData.SetPinMode(PinMode.Input);
            pinLatch.SetPinMode(PinMode.Input);
        }
        return Task.CompletedTask;
    }
}