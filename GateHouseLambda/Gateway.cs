using System;
using System.Collections.Generic;
using System.Text;

namespace GateHouseLambda
{
    public class Gateway
    {
        public string gtw_id { get; set; }
        public long timestamp { get; set; }
        public string time { get; set; }
        public int channel { get; set; }
        public int rssi { get; set; }
        public float snr { get; set; }
        public int rf_chain { get; set; }
    }
}
