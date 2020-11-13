using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GateHouseMonitor
{
    class MP9808I2CDevice
    {


        byte[] buffer;
        I2cDevice device;

        public MP9808I2CDevice()
        {
            buffer = new byte[16];

            I2cConnectionSettings settings = new I2cConnectionSettings(1, 0x18); // busId, Addr-default 0x18
            device = I2cDevice.Create(settings);

        }

        public void read()
        {
            device.Read(buffer.AsSpan<byte>());
        }
    }
}
