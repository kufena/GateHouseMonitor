using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace GateHouseModel
{
    public class GateHouseMonitorModel
    {
        public GateHouseMonitorModel() { }

        public bool OK { get; set; }
        public DateTime Time { get; set; }
        public float Temperature { get; set; }
        public IPAddress[] IPs { get; set; }
        public string Domain { get; set; }
    }
}
