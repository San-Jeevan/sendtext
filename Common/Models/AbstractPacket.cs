using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Models
{

    public class AbstractPacket
    {
        public UpdateType Type { get; set; }
        public string SignalRId { get; set; }
    }
}
