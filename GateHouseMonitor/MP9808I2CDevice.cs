using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GateHouseMonitor
{
    /*
     * MP9808 code to read temperature.
     * 
     * An explaination of sorts.
     * 
     * The way I2C seems to work, from what I can gather, is that a device sits on a bus and has an address.
     * By default, the address for an MP9808 is 0x18 (25 decimanl), and for the Pi, the default pins are
     * for bus 1.  But you can pass in to the c'tor as you may have more than one device on a bus.
     * 
     * To make the device work, you send a command to the device, which
     * is either Read or Write and then an address.  You then either read some bytes or write some bytes.
     * The bit about 'sending the command' is hidden by the System.Device.I2c implementation, as far as I
     * can tell, in its read and write methods.
     * 
     * However, in order to 'read' the temperature, you still need to tell the device what 'register' you
     * want to read.  In this case, we want to read register 0x05, so you write that first, and then
     * read a number of bytes.  The value we want is 12 bits or something, so here we read two bytes.
     * 
     * The values I get back are sort of what I expect, and when I put my fingers on the MP9808 device
     * the temperature goes up to about 31 or 32 degrees C.  However, when reading, the three status bits
     * are giving me weird values, so who knows.  I am using an old document which describes all this,
     * which can be found at https://ww1.microchip.com/downloads/en/DeviceDoc/25095A.pdf if you want to
     * look, but my device is newer than the document and things may have changed, I don't know.
     */
    class MP9808I2CDevice
    {        
        I2cDevice device;

        // Default values on my Pi.
        public MP9808I2CDevice() : this(1, 0x18) { }

        // Pass in the bus and address for the device.
        public MP9808I2CDevice(int busId, int addr)
        {
            I2cConnectionSettings settings = new I2cConnectionSettings(busId, addr); // busId, Addr-default 0x18
            device = I2cDevice.Create(settings);
        }

        /**
         * Returns a temperature in degrees C.
         */
        public float read()
        {
            byte[] buffer = new byte[2];

            // Write the register number we want to read.
            device.WriteByte((byte)0x05);
            // Read the bytes.
            device.Read(buffer.AsSpan<byte>());

            // 0 is upper byte, 1 is lower byte here.
            // The values I get back are ok-ish, but I get the status bits set.
            if ((buffer[0] & 0x80) == 0x80)
            {
                // T_A > T_CRIT
                Console.WriteLine("Numero 1");
            }

            if ((buffer[0] & 0x40) == 0x40)
            {
                // T_A > T_UPPER
                Console.WriteLine("Numero 2");
            }

            if ((buffer[0] & 0x20) == 0x20)
            {
                // T_A < T_LOWER
                Console.WriteLine("Numero 3");
            }

            // The calculation for the temperature is taken from the 25095A.pdf document linked
            // to in the above class comment.
            float temperature = 0.0f;

            byte upperv = (byte) (buffer[0] & 0x1F); // Clear flag bits.

            if ((upperv & 0x10) == 0x10) { // Check sign
                // Negative
                upperv = (byte) (upperv & 0x0F);
                temperature = 256.0f - ((float)upperv * 16f + ((float)buffer[1] / 16f));
            }
            else
            {
                // Positive
                temperature = (float)upperv * 16f + ((float)buffer[1] / 16f);
            }
            return temperature;
        }
    }
}
