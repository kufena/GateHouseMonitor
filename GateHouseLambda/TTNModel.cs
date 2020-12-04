using System;
using System.Collections.Generic;
using System.Text;

namespace GateHouseLambda
{
    public class TTNModel
    {
        public TTNModel() { }

        public string app_id { get; set; }
        public string dev_id { get; set; }
        public string hardware_serial { get; set; }
        public int port { get; set; }
        public int counter { get; set; }
        public string public_raw { get; set; }
        public Payload_Fields payload_fields { get; set; }
        public MetaData metadata { get; set; }
        public string downlink_url { get; set; }
    }
}
