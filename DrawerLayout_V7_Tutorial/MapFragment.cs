using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.Database;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Common.Models;
using DrawerLayout_V7_Tutorial.Models;
using Newtonsoft.Json;
using CursorLoader = Android.Support.V4.Content.CursorLoader;
using Double = System.Double;
using Thread = System.Threading.Thread;

namespace DrawerLayout_V7_Tutorial
{
    class LatLngEvaluator : Java.Lang.Object, ITypeEvaluator
    {
        public Java.Lang.Object Evaluate(float fraction, Java.Lang.Object startValue, Java.Lang.Object endValue)
        {
            var start = (LatLng)startValue;
            var end = (LatLng)endValue;

            return new LatLng(start.Latitude + fraction * (end.Latitude - start.Latitude),
                               start.Longitude + fraction * (end.Longitude - start.Longitude));
        }
    }
    internal class MapFragment : Fragment, IOnMapReadyCallback
    {
        private Android.Gms.Maps.MapFragment mapFragment = null;
        private static GoogleMap nMap;
        static readonly List<LatLng> _recentCordinates = new List<LatLng>();
        private static Marker _myMarker;
        static readonly List<SessionParticipant> _SessionParticipants = new List<SessionParticipant>();


        public static void AnimateMarker(SessionParticipant participant, MarkerOptions markerOptions)
        {
            var marker = participant.Marker;
            var toPosition = markerOptions.Position;
            long start = SystemClock.UptimeMillis();
            Projection proj = nMap.Projection;
            Point startPoint = proj.ToScreenLocation(marker.Position);
            LatLng startLatLng = proj.FromScreenLocation(startPoint);
            var evaluator = new LatLngEvaluator();
            var animator = ObjectAnimator.OfObject(marker, "position", evaluator, startLatLng, toPosition)
                .SetDuration(3000);
            animator.AnimationEnd += Animator_AnimationEnd;
            animator.Start();
        }

        private static void Animator_AnimationEnd(object sender, System.EventArgs e)
        {
            //ObjectAnimator valueAnimator = (ObjectAnimator)sender;
            //SessionParticipant participant = _SessionParticipants.Find(x => x.SignalRId == valueAnimator.PropertyName);
            //var marker = (Marker)valueAnimator.Target;
            //var circleCenter = new MarkerOptions();
            //circleCenter.SetPosition(participant.Marker.Position);
            //participant.Marker = nMap.AddMarker(circleCenter);
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
                if (_recentCordinates.Count == 0)
                {
                    var newcameraFocus = Common.CameraFocus(_SessionParticipants, _myMarker);
                    nMap.AnimateCamera(newcameraFocus);
                }

                _recentCordinates.Add(_newPosition);

            }
        }


        public class ParticipantCordReceiver : BroadcastReceiver
        {

            public override void OnReceive(Context context, Intent intent)
            {
                string message = intent.GetStringExtra("updPacket");

                if (nMap == null) return;
                var gpsPacket = JsonConvert.DeserializeObject<LocationUpdPacket>(message);
                LatLng _newPosition = new LatLng(gpsPacket.Latitude, gpsPacket.Longitude);
                var circleCenter = new MarkerOptions();
                circleCenter.SetPosition(_newPosition);


                var existingMarker = _SessionParticipants.FirstOrDefault(x => x.SignalRId == gpsPacket.SignalRId);
                if (existingMarker != null)
                {
                    //if Visible is not set to false before remove, it will leave a whitespace behind.Bug!
                    existingMarker.Marker.Visible = false;
                    existingMarker.Marker.Remove();
                    existingMarker.Marker = nMap.AddMarker(circleCenter);

                    //TODO: Marker endanimation show new position
                    //AnimateMarker(existingMarker, circleCenter);
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
                    if (_myMarker != null)
                    {
                        var newcameraFocus = Common.CameraFocus(_SessionParticipants, _myMarker);
                        nMap.AnimateCamera(newcameraFocus);
                    }

                }
            }


        }

        public class ParticipantStatusReceiver : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                string rawmessage = intent.GetStringExtra("updPacket");
                var message = JsonConvert.DeserializeObject<StatusUpdPacket>(rawmessage);
                if (message.Status == StatusType.Disconnected)
                {
                    for (var i = 0; i < _SessionParticipants.Count; i++)
                    {
                        if (_SessionParticipants[i].SignalRId == message.SignalRId)
                        {
                            var existingMarker = _SessionParticipants[i];
                            if (existingMarker != null)
                            {
                                existingMarker.Marker.Visible = false;
                                existingMarker.Marker.Remove();
                            }
                            _SessionParticipants.Remove(existingMarker);
                        }
                    }
                }

                else if (message.Status == StatusType.Connected)
                {
                    //SendLocationToInternet({ coords: { latitude: _myMarker.position.lat(), longitude: _myMarker.position.lng() } });
                }

            }
        }


        private void SlowMethod()
        {
            Thread.Sleep(1000);
            mapFragment = new Android.Gms.Maps.MapFragment();

            this.Activity.RunOnUiThread(() => mapFragment.GetMapAsync(this));
            FragmentManager.BeginTransaction().Replace(Resource.Id.mapslayout, mapFragment).Commit();


            //OWN LOCATION UPDATES
            var myReceiver = new MyCordinatesReceiver();
            LocalBroadcastManager.GetInstance(this.Activity).RegisterReceiver(
               myReceiver, new IntentFilter("PosUpdate"));

            //PARTICIPANT LOCATION UPDATES
            var participantReceiver = new ParticipantCordReceiver();
            LocalBroadcastManager.GetInstance(this.Activity).RegisterReceiver(
               participantReceiver, new IntentFilter("ParticipantPosUpdate"));

            //PARTICIPANT STATUS UPDATES
            var participantStatusReceiver = new ParticipantStatusReceiver();
            LocalBroadcastManager.GetInstance(this.Activity).RegisterReceiver(
               participantStatusReceiver, new IntentFilter("ParticipantStatusUpdate"));


        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            ThreadPool.QueueUserWorkItem(o => SlowMethod());
            base.OnViewCreated(view, savedInstanceState);
        }

        private bool IsServiceRunning(string myservice)
        {
            var manager = (ActivityManager)this.Activity.GetSystemService(Context.ActivityService);
            var mylist = manager.GetRunningServices(int.MaxValue).Where(
                service => service.Service.ClassName.Contains(myservice));
            if (mylist.Count() != 0) return true;
            return false;
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (container == null)
            {
                // Currently in a layout without a container, so no reason to create our view.
                return null;
            }
            return inflater.Inflate(Resource.Layout.MapsLayout, container, false);
        }


        public void OnMapReady(GoogleMap googleMap)
        {
            nMap = googleMap;
        }

        public override void OnResume()
        {
            var alreadyrunning = IsServiceRunning("GPSservice");
            if (!alreadyrunning)
            {
                Android.App.Application.Context.StartService(new Intent(this.Activity, typeof(GPSservice)));
            }
            base.OnResume();
        }



        private void JobTypeListClicked(object sender, DialogClickEventArgs eventargs)
        {
            var test = "sdds";
        }

        private List<string> GetContactList()
        {
            var uri = ContactsContract.CommonDataKinds.Phone.ContentUri.BuildUpon()
        .AppendQueryParameter(ContactsContract.RemoveDuplicateEntries, "1")
        .Build();
            List<string> contacts = new List<string>();
            string[] projection = {
                             ContactsContract.Contacts.InterfaceConsts.Id,
                              ContactsContract.Contacts.InterfaceConsts.DisplayName,
                         ContactsContract.CommonDataKinds.Phone.Number
            };
            var loader = new CursorLoader(this.Activity, uri, projection, null, null, null);
            var cursor = (ICursor)loader.LoadInBackground();

            cursor.MoveToFirst();
            do
            {
                var ContactId = cursor.GetLong(cursor.GetColumnIndex(projection[0]));
                var Name = cursor.GetString(cursor.GetColumnIndex(projection[1]));
                var number = cursor.GetString(cursor.GetColumnIndex(projection[2]));
                contacts.Add(Name);
                Log.Debug("smedrix", Name + " " + number);

            } while (cursor.MoveToNext());
            return contacts;
        }

        public void AddParticipantBtnClicked()
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this.Activity);
            builder.SetTitle("Select participant to invite");
            var initialcontacts = GetContactList();
            string[] contacts = initialcontacts.Select(o => o).ToArray();
            builder.SetSingleChoiceItems(contacts, -1, JobTypeListClicked);

            builder.SetNegativeButton("Cancel", (sender, args) =>
            {

            });

            AlertDialog dialog = builder.Create();
            dialog.Show();
        }
    }
}