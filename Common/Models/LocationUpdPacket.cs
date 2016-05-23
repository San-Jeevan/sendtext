using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Models
{

    public class LocationUpdPacket : AbstractPacket
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
