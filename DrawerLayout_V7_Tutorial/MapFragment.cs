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
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace DrawerLayout_V7_Tutorial
{

    internal class MapFragment : Fragment, IOnMapReadyCallback
    {

        private static GoogleMap nMap;
        static readonly List<LatLng> _recentCordinates = new List<LatLng>();
        private static Marker _myMarker;

        [BroadcastReceiver(Enabled = true)]
      
        public class MyTestReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                Double currentLatitude = intent.GetDoubleExtra("latitude", 0);
                Double currentLongitude = intent.GetDoubleExtra("longitude", 0);
                LatLng _newPosition = new LatLng(currentLatitude, currentLongitude);
                if (_recentCordinates.Count==0) nMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(_newPosition, 15));

                var circleCenter = new MarkerOptions();
                circleCenter.SetPosition(_newPosition);
                circleCenter.SetSnippet("decdssf dsfdsfds");
                circleCenter.SetTitle("yolo");
                if(_myMarker!= null) _myMarker.Remove();
                _myMarker = nMap.AddMarker(circleCenter);
                _recentCordinates.Add(_newPosition);
            }
        }


        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            //SetUpMap();
            var receiver = new MyTestReceiver();
            LocalBroadcastManager.GetInstance(this.Activity).RegisterReceiver(
               receiver, new IntentFilter("PosUpdate"));
            base.OnViewCreated(view, savedInstanceState);
        }

        private bool IsServiceRunning(string myservice)
        {
            var manager = (ActivityManager)this.Activity.GetSystemService(Context.ActivityService);
            var mylist = manager.GetRunningServices(int.MaxValue).Where(
                service => service.Service.ClassName.Contains(myservice));
            if(mylist.Count()!= 0) return true;
            return false;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (container == null)
            {
                // Currently in a layout without a container, so no reason to create our view.
                return null;
            }

            var alreadyrunning = IsServiceRunning("GPSservice");
            if(!alreadyrunning) this.Activity.StartService(new Intent(this.Activity, typeof(GPSservice)));
            return inflater.Inflate(Resource.Layout.MapsLayout, container, false);
        }


        private void SetUpMap()
        {
            if (nMap == null)
            {
                ChildFragmentManager.FindFragmentById<Android.Gms.Maps.MapFragment>(Resource.Id.map).GetMapAsync(this);
            }
        }
        public void OnMapReady(GoogleMap googleMap)
        {
            nMap = googleMap;
        }
    }
       
}