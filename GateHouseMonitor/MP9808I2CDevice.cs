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
        
        I2cDevice device;

        public MP9808I2CDevice(int busId, int addr)
        {
            I2cConnectionSettings settings = new I2cConnectionSettings(busId, addr); // busId, Addr-default 0x18
            device = I2cDevice.Create(settings);
        }

        public void read(byte[] buffer)
        {
            device.Read(buffer.AsSpan<byte>());
            buffer[buffer.Length - 1] = device.ReadByte();

        }
    }
}
