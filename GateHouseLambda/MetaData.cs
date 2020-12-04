using System;
using System.Collections.Generic;
using System.Text;

namespace GateHouseLambda
{
    public class MetaData
    {
        public string time { get; set; }
        public float frequency { get; set; }
        public string modulation { get; set; }
        public string data_rate { get; set; }
        public string coding_rate { get; set; }
        public List<Gateway> gateways { get; set; }
    }
}
