using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace GateHouseModel
{
    public class GateHouseMonitorModel
    {
        public GateHouseMonitorModel() { }
        public GateHouseMonitorModel(bool b, DateTime t, float f, IPAddress[] i)
        {
            this.OK = b;
            this.Time = t;
            this.Temperature = f;
            this.IPs = i;
        }

        public bool OK { get; set; }
        public DateTime Time { get; set; }
        public float Temperature { get; set; }
        public IPAddress[] IPs { get; set; }
    }
}
