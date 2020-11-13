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

        public float read(byte[] buffer)
        {
            device.WriteByte((byte)0x05);
            device.Read(buffer.AsSpan<byte>());

            // 0 is upper byte, 1 is lower byte here.
            if ((buffer[0] & 0x80) == 0x80)
            {
                Console.WriteLine("Numero 1");
            }

            if ((buffer[0] & 0x40) == 0x40)
            {
                Console.WriteLine("Numero 2");
            }

            if ((buffer[0] & 0x20) == 0x20)
            {
                Console.WriteLine("Numero 3");
            }

            float temperature = 0.0f;

            byte upperv = (byte) (buffer[0] & 0x1F);
            if ((upperv & 0x10) == 0x10) {
                upperv = (byte) (upperv & 0x0F);
                temperature = 256.0f - ((float)upperv * 16f + ((float)buffer[1] / 16f));
            }
            else
            {
                temperature = (float)upperv * 16f + ((float)buffer[1] / 16f);
            }
            return temperature;
        }
    }
}
