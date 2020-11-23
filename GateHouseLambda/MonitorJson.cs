using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace GateHouseLambda
{
    public class MonitorJson
    {
        public MonitorJson() {  }

        public bool OK { get; set; }
        public DateTime Time { get; set; }
        public float Temperature { get; set; }
        public string CallerIP { get; set; }
        public IPAddress[] DomainIP { get; set; }
    }
}
