using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using GateHouseModel;
using System.Threading.Tasks;
using System.Device.Gpio;
using System.Device.I2c;
using System.Text.Json;

namespace GateHouseMonitor
{
    class Program
    {
        /**
         * arguments:
         *   URL of Lambda: compulsory
         *   Period in minutes
         *   Bus id
         *   Address
         *   
         *   In that order.  Really ought to make a better job of this shouldn't I.
         */
        public static async Task Main(string[] args)
        {
            string url = args[0];
            int busid = 1;
            int addr = 0x18;
            int period = 15 * 60 * 1000;
            bool success = true;

            if (args.Length > 1)
                period = Int32.Parse(args[1]) * 60 * 1000;

            if (args.Length > 2)
            {
                busid = Int32.Parse(args[2]);
                addr = Int32.Parse(args[3]);
            }

            Console.WriteLine("Gate House Monitor Starting.");
            Console.WriteLine($"Using API URI of {url}");
            Console.WriteLine($"Checking device on bus {busid} and address {addr} every {period} milliseconds.");
            Stopwatch sp = new Stopwatch();
            //MP9808I2CDevice device = new MP9808I2CDevice(busid, addr);
            
            sp.Start();

            var dt = DateTime.Now;
            IPAddress[] amcrestIp;
            try
            {
                amcrestIp = Dns.GetHostAddresses("amcrestcloud.com");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                amcrestIp = new IPAddress[] { };
                success = false;
            }

            float temp = 0f; // device.read();
            Console.WriteLine($"Temperature = {temp}");

            var model = new GateHouseMonitorModel
            {
                OK = success && (amcrestIp.Length > 0),
                Time = dt.ToLocalTime(),
                Temperature = temp,
                IPs = amcrestIp
            };

            HttpResponseMessage response = await sendData(url, model);
            Console.WriteLine($"Here we go! {amcrestIp.Length} - {sp.Elapsed} - {response.StatusCode}");

        }

        private static async Task<HttpResponseMessage> sendData(string url, GateHouseMonitorModel model)
        {
            JsonSerializerOptions opts = new JsonSerializerOptions();
            opts.Converters.Add(new JsonDateTimeConverter());
            opts.Converters.Add(new JsonIPAddressConverter());
            HttpRequestMessage msg = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
                Content = JsonContent.Create<GateHouseMonitorModel>(model, options: opts)
            };
            
            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, 30);
            var response = await client.SendAsync(msg);
            return response;
        }
    }
}
