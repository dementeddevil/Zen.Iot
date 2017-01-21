using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.I2c;
using Windows.Web.Syndication;
using Porrey.Uwp.IoT.Sensors;

namespace Zen.Iot.Devices.Csb502Ssd
{
    public sealed class Eeprom
    {
        public async Task<bool> SetupUsbHub()
        {
            using (var device = new I2c(0x50))
            {
                // Initialise device
                var result = await device.InitializeAsync().ConfigureAwait(false);
                if (result != InitializationResult.Successful)
                {
                    return false;
                }

                // Setup memory block to write to I2C EEPROM
                using (var stream = new MemoryStream())
                {
                    // Setup EEPROM contents
                    // This is used to initialise the 4-Port USB Hub so our
                    //  driver can detect it later...
                    using (var writer = new BinaryWriter(stream, Encoding.Unicode, true))
                    {
                        writer.Write((ushort)0xf0f1);   // Vendor ID
                        writer.Write((ushort)0x34ad);   // Product ID
                        writer.Write((ushort)0x0000);   // Device ID

                        writer.Write((byte)0x90);       // Config Data Byte 1 [self-powered & multi TT per port op]
                        writer.Write((byte)0x08);       // Config Data Byte 2 [compound device]
                        writer.Write((byte)0x00);       // Config Data Byte 3

                        writer.Write((byte)0x1E);       // Non-Removable Devices [all devices are non-removable]

                        writer.Write((byte)0x00);       // Port disable (self) [all ports enabled]
                        writer.Write((byte)0x00);       // Port disable (bus) [all ports enabled]
                        writer.Write((byte)0x05);       // Max power (self) [10mA]
                        writer.Write((byte)0x05);       // Max power (bus) [10mA]
                        writer.Write((byte)0x20);       // Hub Controller Max Current (self) [64mA]
                        writer.Write((byte)0x20);       // Hub Controller Max Current (bus) [64mA]
                        writer.Write((byte)0x80);       // Power-on time [256ms]

                        writer.Write((ushort)0x0809);   // Language ID [en-gb]

                        var manufacturer = "Zen Design Software";
                        var product = "4 Port Hub";
                        var serial = Guid.NewGuid().ToString("N");
                        writer.Write((byte)manufacturer.Length * 2);                // Manufacturer string length
                        writer.Write((byte)product.Length * 2);                     // Product string length
                        writer.Write((byte)serial.Length * 2);                      // Serial string length
                        writer.Write(Encoding.Unicode.GetBytes(manufacturer));      // Manufacturer unicode string
                        writer.Write(Encoding.Unicode.GetBytes(product));           // Product unicode string
                        writer.Write(Encoding.Unicode.GetBytes(serial));            // Serial unicode string

                        writer.Write((byte)0x00);       // Battery charging enable
                        writer.Write((byte)0x00);       // Reserved
                        writer.Write((byte)0x00);       // Reserved

                        writer.Write((byte)0x00);       // Boost up
                        writer.Write((byte)0x00);       // Reserved

                        writer.Write((byte)0x00);       // Boost x
                        writer.Write((byte)0x00);       // Reserved

                        writer.Write((byte)0x00);       // Port swap
                        writer.Write((byte)0x00);       // Port map 12
                        writer.Write((byte)0x00);       // Port map 34
                    }

                    // Write memory stream to EEPROM
                    ArraySegment<byte> buffer;
                    if (!stream.TryGetBuffer(out buffer))
                    {
                        return false;
                    }

                    await device.WriteAsync(buffer.Array).ConfigureAwait(false);
                }

                return true;
            }
        }
    }
}
