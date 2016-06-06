using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DrawerLayout_V7_Tutorial.Models;

namespace DrawerLayout_V7_Tutorial
{
    public static class Common
    {
        public static CameraUpdate CameraFocus(List<SessionParticipant> participants, Marker myMarker)
        {
            if (participants.Count == 0) return CameraUpdateFactory.NewLatLngZoom(myMarker.Position, 15);
            LatLngBounds.Builder builder = new LatLngBounds.Builder();
            builder.Include(myMarker.Position);
            for (int i = 0; i < participants.Count; i++)
            {
                builder.Include(participants[i].Marker.Position);
            }
            LatLngBounds bounds = builder.Build();
           
            CameraUpdate cu = CameraUpdateFactory.NewLatLngBounds(bounds, 5);
            return cu;
        }
    }
}