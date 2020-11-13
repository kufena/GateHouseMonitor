using System;
using System.Collections.Generic;
using System.Text;

namespace GateHouseModel
{
    public class GateHouseMonitorModel
    {
        public GateHouseMonitorModel() { }
        public GateHouseMonitorModel(bool b, DateTime t, float f)
        {
            this.OK = b;
            this.Time = t;
            this.Temperature = f;
        }

        public bool OK { get; set; }
        public DateTime Time { get; set; }
        public float Temperature { get; set; }
    }
}
