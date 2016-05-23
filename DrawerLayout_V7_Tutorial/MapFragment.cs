using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Common.Models;
using DrawerLayout_V7_Tutorial.Models;
using Java.Text;
using Newtonsoft.Json;

namespace DrawerLayout_V7_Tutorial
{

    internal class MapFragment : Fragment, IOnMapReadyCallback
    {

        private static GoogleMap nMap;
        static readonly List<LatLng> _recentCordinates = new List<LatLng>();
        private static Marker _myMarker;
        static readonly List<SessionParticipant> _SessionParticipants = new List<SessionParticipant>();

        public static string CalculationByDistance(LatLng StartP, LatLng EndP)
        {
            float[] result = new float[1];
            Android.Locations.Location.DistanceBetween(StartP.Latitude, StartP.Longitude, EndP.Latitude, EndP.Longitude, result);
            return result[0].ToString("0.0");
        }


        [BroadcastReceiver(Enabled = true)]
        public class MyCordinatesReceiver : BroadcastReceiver
        {

            public override void OnReceive(Context context, Intent intent)
            {
                if (nMap == null) return;
                Double currentLatitude = intent.GetDoubleExtra("latitude", 0);
                Double currentLongitude = intent.GetDoubleExtra("longitude", 0);
                LatLng _newPosition = new LatLng(currentLatitude, currentLongitude);
                if (_recentCordinates.Count==0) nMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(_newPosition, 15));

                var circleCenter = new MarkerOptions();
                
                circleCenter.SetPosition(_newPosition);
                circleCenter.SetSnippet("decdssf dsfdsfds");
                circleCenter.SetTitle("yolo");

                if (_myMarker != null)
                {
                    _myMarker.Visible = false;
                    _myMarker.Remove();
                }
                _myMarker = nMap.AddMarker(circleCenter);
                _recentCordinates.Add(_newPosition);
            }
        }


        public class ParticipantCordReceiver : BroadcastReceiver
        {

            public override void OnReceive(Context context, Intent intent)
            {
                if (nMap == null) return;
                string message = intent.GetStringExtra("updPacket");
                var gpsPacket = JsonConvert.DeserializeObject<LocationUpdPacket>(message);

                LatLng _newPosition = new LatLng(gpsPacket.Latitude, gpsPacket.Longitude);
                var circleCenter = new MarkerOptions();
                circleCenter.SetPosition(_newPosition);

               

                var existingMarker = _SessionParticipants.FirstOrDefault(x => x.SignalRId == gpsPacket.SignalRId);
                if (existingMarker != null)
                {
                    //if Visible is not set to false before remove, it will leave a whitespace behind. Bug!
                    existingMarker.Marker.Visible=false;
                    existingMarker.Marker.Remove();
                    existingMarker.Marker = nMap.AddMarker(circleCenter);
                }
                else
                {
                    _SessionParticipants.Add(new SessionParticipant()
                    {
                        Marker = nMap.AddMarker(circleCenter),
                        SignalRId = gpsPacket.SignalRId,
                        Username = "lol"
                    });

                    //zoom in to show both markers, initial
                    LatLngBounds.Builder builder = new LatLngBounds.Builder();
                    builder.Include(_myMarker.Position);
                    builder.Include(_newPosition);
                    LatLngBounds bounds = builder.Build();
                    CameraUpdate cu = CameraUpdateFactory.NewLatLngBounds(bounds, 100);
                    nMap.AnimateCamera(cu);

                }
               var distanceInMeter= CalculationByDistance(_newPosition, _myMarker.Position);
            }
        }


        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            SetUpMap();
            var myReceiver = new MyCordinatesReceiver();
            LocalBroadcastManager.GetInstance(this.Activity).RegisterReceiver(
               myReceiver, new IntentFilter("PosUpdate"));
            var participantReceiver = new ParticipantCordReceiver();
            LocalBroadcastManager.GetInstance(this.Activity).RegisterReceiver(
               participantReceiver, new IntentFilter("ParticipantPosUpdate"));
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
                if (Build.VERSION.SdkInt > BuildVersionCodes.Kitkat)
                {
                    ChildFragmentManager.FindFragmentById<Android.Gms.Maps.MapFragment>(Resource.Id.map).GetMapAsync(this);
                }
                else
                {
                    FragmentManager.FindFragmentById<Android.Gms.Maps.MapFragment>(Resource.Id.map).GetMapAsync(this);
                }
            
            }
        }
        public void OnMapReady(GoogleMap googleMap)
        {
            nMap = googleMap;
        }
    }
       
}