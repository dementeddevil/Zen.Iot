using System;
using System.Collections.Generic;
using Windows.Devices.Gpio;
using Windows.Devices.I2c;

namespace Zen.Iot.Devices.Csb502Ssd
{
    /// <summary>
    /// <c>Ds1339</c> Real-time clock on I2C bus device.
    /// </summary>
    /// <remarks>
    /// Reference notes for CSB502SSD implementation:
    /// 1. The CSB502SSD is fitted with a CR1620 Battery rated at 75ma/hr.
    ///     This is a non-rechargeable battery.
    ///     It will provide back-up power (when the Raspberry PI is non-powered) for 10+ years.
    ///     Temperature and humidity beyond the ratings for the CSB502SSD can shorten this life considerably. 
    /// 2. By default the DS1339 RTC interrupt is set to a 1 second square wave
    ///     with the output disabled.
    ///     Software should setup the interrupt function for the desired functionality.
    ///     Refer to the DS1339 data sheet for more information. 
    /// 3. The DS1339 supports a rechargeable battery using an internal trickle charger.
    ///     This is disabled by default and should only be used if the coin cell has been replaced
    ///     with a verified rechargeable battery. Enabling the trickle charger (valid only when the
    ///     3.3V rail is on) with a primary (nonrechargeable) coin cell will not damage the coin cell,
    ///     but may drain it faster. 
    /// 4. The device is placed at address 0x68 on the Raspberry PI I2C bus. Additionally the interrupt
    ///     output from the DS1339 RTC is presented at GPIO pin 20.
    /// </remarks>
    public sealed class Ds1339 : IDisposable
    {
        private enum Register
        {
            ClockSeconds = 0x00,
            ClockMinutes = 0x01,
            ClockHours = 0x02,
            ClockDayOfWeek = 0x03,
            ClockDay = 0x04,
            ClockMonth = 0x05,
            ClockYear = 0x06,
            Alarm1Seconds = 0x07,
            Alarm1Minutes = 0x08,
            Alarm1Hours = 0x09,
            Alarm1DayOrDate = 0x0A,
            Alarm2Minutes = 0x0B,
            Alarm2Hours = 0x0C,
            Alarm2DayOrDate = 0x0D,
            Control = 0x0E,
            Status = 0x0F,
            TrickleCharger = 0x10
        }

        private const ushort I2CAddress = 0x0068;
        private const int DeviceMemoryBlockSize = 17;

        private bool _isDisposed;
        private I2cDevice _device;
        private GpioPin _interruptPin;
        private readonly HashSet<Action> _interruptHandlers = new HashSet<Action>();

        private byte[] _write1 = new byte[1];
        private byte[] _write2 = new byte[2];
        private byte[] _read1 = new byte[1];

        public Ds1339(I2cDevice device, GpioPin interruptPin)
        {
            _device = device;
            _interruptPin = interruptPin;
            _interruptPin.ValueChanged += OnRaiseInterrupts;
        }

        ~Ds1339()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void SetTimeAndDate(DateTime date)
        {
            ThrowIfDisposed();

            WriteRegister(
                Register.ClockSeconds,
                (byte)(((date.Second / 10) << 4) | ((date.Second % 10) & 0x0f)));
            WriteRegister(
                Register.ClockMinutes,
                (byte)(((date.Minute / 10) << 4) | ((date.Minute % 10) & 0x0f)));
            WriteRegister(
                Register.ClockHours,
                (byte)(((date.Hour / 10) << 4) | ((date.Hour % 10) & 0x0f)));

            byte dayOfWeek = (byte)date.DayOfWeek;
            ++dayOfWeek;
            WriteRegister(
                Register.ClockDayOfWeek,
                dayOfWeek);

            WriteRegister(
                Register.ClockDay,
                (byte)(((date.Day / 10) << 4) | ((date.Day % 10) & 0x0f)));
            WriteRegister(
                Register.ClockMonth,
                (byte)(((date.Month / 10) << 4) | ((date.Month % 10) & 0x0f)));

            int centuryYear = date.Year % 100;
            WriteRegister(
                Register.ClockYear,
                (byte)(((centuryYear / 10) << 4) | ((centuryYear % 10) & 0x0f)));
        }

        private void OnRaiseInterrupts(GpioPin pin, GpioPinValueChangedEventArgs e)
        {
            if (e.Edge != GpioPinEdge.RisingEdge)
            {
                return;
            }

            foreach (var handler in _interruptHandlers)
            {
                handler();
            }            
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _device?.Dispose();
                    _interruptPin.ValueChanged -= OnRaiseInterrupts;
                    _interruptPin.Dispose();
                }
                _isDisposed = true;
            }
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }

        private void WriteRegister(Register register, byte value)
        {
            _write2[0] = (byte)register;
            _write2[1] = value;

            _device.Write(_write2);
        }

        private byte ReadRegister(Register register)
        {
            _write1[0] = (byte)register;

            _device.WriteRead(_write1, _read1);

            return _read1[0];
        }
    }
}
