using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Gms.Maps.Model;

namespace DrawerLayout_V7_Tutorial.Models
{

    public class SessionParticipant
    {
        public Marker Marker { get; set; }
        public string SignalRId { get; set; }
        public string Username { get; set; }
    }
}
