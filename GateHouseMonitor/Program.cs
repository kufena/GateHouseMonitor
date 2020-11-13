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

namespace GateHouseMonitor
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            string url = args[0];
            int sz = Int32.Parse(args[1]);
            int busid = Int32.Parse(args[2]);
            int addr = Int32.Parse(args[3]);

            Console.WriteLine("Gate House Monitor Starting.");
            Console.WriteLine($"Using API URI of {url}");

            Stopwatch sp = new Stopwatch();
            MP9808I2CDevice device = new MP9808I2CDevice(busid, addr);

            sp.Start();
            while(true)
            {

                var dt = DateTime.Now;
                var amcrestIp = Dns.GetHostAddresses("amcrestcloud.com");
                var model = new GateHouseMonitorModel
                {
                    OK = (amcrestIp.Length > 0),
                    Time = dt
                };

                byte[] buffer = new byte[sz];
                device.read(buffer);

                string s = "buffer::";
                for (int i = 0; i < sz; i++)
                    s += " " + ((uint)buffer[i]);
                Console.WriteLine(s);
                //HttpResponseMessage response = await sendData(url, model);
                //Console.WriteLine($"Here we go! {amcrestIp.Length} - {sp.Elapsed} - {response.StatusCode}");
                Thread.Sleep(15 * 1 * 1000);

            }
        }

        private static async Task<HttpResponseMessage> sendData(string url, GateHouseMonitorModel model)
        {
            HttpRequestMessage msg = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
                Content = JsonContent.Create<GateHouseMonitorModel>(model)
            };

            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, 30);
            var response = await client.SendAsync(msg);
            return response;
        }
    }
}
