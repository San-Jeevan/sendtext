using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Models
{

    public class StatusUpdPacket : AbstractPacket
    {
        public StatusType Status { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return "{" + String.Format("\"SignalRId\": \"{0}\", \"Type\": {1}, \"Status\": {2} , \"Description\": \"{3}\"", this.SignalRId, (int)this.Type, (int)this.Status, this.Description) + "}";
        }
    }
}
