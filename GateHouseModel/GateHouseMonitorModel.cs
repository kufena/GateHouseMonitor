using System;
using System.Collections.Generic;
using System.Text;

namespace GateHouseModel
{
    public class GateHouseMonitorModel
    {
        public GateHouseMonitorModel() { }
        public GateHouseMonitorModel(bool b, DateTime t)
        {
            this.OK = b;
            this.Time = t;
        }
        public bool OK { get; set; }
        public DateTime Time { get; set; }
    }
}
