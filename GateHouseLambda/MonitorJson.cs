using System;
using System.Collections.Generic;
using System.Text;

namespace GateHouseLambda
{
    public class MonitorJson
    {
        public MonitorJson() {  }

        public bool OK { get; set; }
        public DateTime Time { get; set; }
        public float Temperature { get; set; }
        public string IP { get; set; }
    }
}
