using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreBluetooth;
using CoreLocation;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using UIKit;

namespace iOS
{



    public class GpsService
    {
        HubConnection hubConnection;
        IHubProxy hubProxy;
        private string lastPosition = "";
        private static CLLocationManager locMgr;


        public void OnLocationChanged(object sender, CLLocationsUpdatedEventArgs location)
        {
            Console.WriteLine(location.Locations.FirstOrDefault().Coordinate.Longitude);
            Console.WriteLine(location.Locations.FirstOrDefault().Coordinate.Latitude);
            Console.WriteLine(location.Locations.Length);
        }


        public void startGps()
        {
            // Setting location priority to PRIORITY_HIGH_ACCURACY (100)
            locMgr = new CLLocationManager();
            locMgr.PausesLocationUpdatesAutomatically = false;

            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                locMgr.RequestAlwaysAuthorization(); 
            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                locMgr.AllowsBackgroundLocationUpdates = true;
            }

            if (CLLocationManager.LocationServicesEnabled)
            {
                locMgr.DesiredAccuracy = 1;
                //locMgr.LocationsUpdated += OnLocationChanged;
                locMgr.DistanceFilter = CLLocationDistance.FilterNone;
                locMgr.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
                {
                    Destroy();
                };
                locMgr.StartUpdatingLocation();
            }
          

        }

        public void Destroy()
        {
                new Task(() => {
                    hubConnection.Stop();
                    hubConnection.Dispose();
                }).Start();
        }


        public void Start()
        {
            hubConnection = new HubConnection("http://ws2.gpsfix.io");
            hubProxy = hubConnection.CreateHubProxy("GpsHub");
            hubProxy.On<string>("LocationUpdate", OnSignalRMessage);
            hubConnection.StateChanged += HubConnection_StateChanged;


            hubConnection.Error += ex => Console.WriteLine("An error occurred {0}", ex.Message);

            hubConnection.Closed += () => Console.WriteLine("Connection with client id {0} closed", hubConnection.ConnectionId);


            hubConnection.Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                   
                    Console.WriteLine("Failed to start: {0}", task.Exception.GetBaseException());
                }
                else
                {
                    Console.WriteLine("Success! Connected with client connection id {0}", hubConnection.ConnectionId);
                    startGps();
                    // Do more stuff here
                }
            });

        }


        private void HubConnection_StateChanged(StateChange stateChange)
        {
            if (stateChange.NewState == ConnectionState.Connected)
            {
                hubProxy.Invoke("JoinSession", "testroom");

            }
            if (stateChange.NewState == ConnectionState.Disconnected) return;
            if (stateChange.NewState == ConnectionState.Reconnecting) return;
            if (stateChange.NewState == ConnectionState.Connecting) return;
        }


        private void OnSignalRMessage(string message)
        {
            //var abstractpacket = JsonConvert.DeserializeObject<AbstractPacket>(message);
            ////ignore our own packet.
            //if (abstractpacket.SignalRId == hubConnection.ConnectionId) return;

            ////GPS POSITION UPDATE
            //if (abstractpacket.Type == UpdateType.GpsUpdate)
            //{
            //    Intent intent = new Intent("ParticipantPosUpdate");
            //    SendOthersLocationBroadcast(intent, message);
            //}

            ////STATUSUPDATE (CONNECTED/DISCONNECTED PARTICIPANT)
            //if (abstractpacket.Type == UpdateType.StatusUpdate)
            //{
            //    Intent intent = new Intent("ParticipantStatusUpdate");
            //    SendOthersLocationBroadcast(intent, message);

            //    Intent intenttwo = new Intent("PosUpdate");
            //    SendLocationToInternet(lastPosition.Latitude, lastPosition.Longitude);
            //}

        }

    }
}

