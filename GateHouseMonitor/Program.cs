using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Net;

namespace GateHouseMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Gate House Monitor Starting.");
            Stopwatch sp = new Stopwatch();
            sp.Start();
            while(true)
            {
                Thread.Sleep(15 * 1 * 1000);
                var amcrestIp = Dns.GetHostAddresses("amcrestcloud.com");
               
                Console.WriteLine($"Here we go! {amcrestIp.Length} - {sp.Elapsed}");
            }
        }
    }
}
