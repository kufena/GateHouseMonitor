using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using GateHouseModel;
using System.Threading.Tasks;

namespace GateHouseMonitor
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            string url = args[0];
            //string apikey = "";

            Console.WriteLine("Gate House Monitor Starting.");
            Console.WriteLine($"Using API URI of {url}");

            Stopwatch sp = new Stopwatch();
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

                HttpRequestMessage msg = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Post,
                    Content = JsonContent.Create<GateHouseMonitorModel>(model)
                };

                HttpClient client = new HttpClient();
                client.Timeout = new TimeSpan(0, 0, 2);
                var response = await client.SendAsync(msg);
                Console.WriteLine($"Here we go! {amcrestIp.Length} - {sp.Elapsed} - {response.StatusCode}");
                Thread.Sleep(15 * 1 * 1000);

            }
        }
    }
}
