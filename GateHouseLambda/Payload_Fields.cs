using System;
using System.Collections.Generic;
using System.Text;

namespace GateHouseLambda
{
    public class Payload_Fields
    {
        public Payload_Fields() { }
        public int battery { get; set; }
        public int humidity { get; set; }
        public int pressure { get; set; }
        public int temperature { get; set; }
    }
}
