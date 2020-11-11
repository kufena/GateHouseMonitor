using System;
using System.Collections.Generic;
using System.Text;

namespace GateHouseModel
{
    public class GateHouseMonitorModel
    {
        bool ok;
        DateTime time;

        public GateHouseMonitorModel() { }
        public GateHouseMonitorModel(bool b, DateTime t)
        {
            this.ok = b;
            this.time = t;
        }
        public bool OK { get => ok; set => ok = value; }
        public DateTime Time { get => time; set => time = value; }
    }
}
