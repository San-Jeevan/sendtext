using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Common.Models;
using CoreBluetooth;
using CoreLocation;
using Foundation;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using UIKit;

namespace iOS
{



    public static class GpsService
    {
        static HubConnection hubConnection;
        static IHubProxy hubProxy;
        private static string lastPosition = "";
        static CLLocationManager locMgr =null;



        public static void startGps()
        {
            locMgr = new CLLocationManager();
            locMgr.PausesLocationUpdatesAutomatically = false;
            if (UIDevice.CurrentDevice.CheckSystemVersion(6, 0))
            {
                locMgr.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
                {
                    var l = e.Locations[e.Locations.Length-1];
                    if (hubConnection.State == ConnectionState.Connected)
                    {
                        var update = new LocationUpdPacket { SignalRId = hubConnection.ConnectionId, Latitude = l.Coordinate.Latitude, Longitude = l.Coordinate.Longitude, Type = UpdateType.GpsUpdate };
                        object[] wordsToSend = new object[] { "testroom", JsonConvert.SerializeObject(update) };
                        hubProxy.Invoke("sendSessionMessage", wordsToSend);
                    }
                };
            }
            else
            {
                locMgr.UpdatedLocation += (object sender, CLLocationUpdatedEventArgs e) =>
                {
                    var l = e.NewLocation;
                    if (hubConnection.State == ConnectionState.Connected)
                    {
                        var update = new LocationUpdPacket { SignalRId = hubConnection.ConnectionId, Latitude = l.Coordinate.Latitude, Longitude = l.Coordinate.Longitude, Type = UpdateType.GpsUpdate };
                        object[] wordsToSend = new object[] { "testroom", JsonConvert.SerializeObject(update) };
                        hubProxy.Invoke("sendSessionMessage", wordsToSend);
                    }
                };

            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                locMgr.RequestWhenInUseAuthorization();
            }

            if (CLLocationManager.LocationServicesEnabled)
            locMgr.StartUpdatingLocation();

        }

        public static void Destroy()
        {
            var myDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
            myDelegate.InvokeOnMainThread(() =>
            {
                locMgr.StopUpdatingLocation();
                locMgr.Dispose();
                locMgr = null;
            });

            new Task(() => {
                    hubConnection.Stop();
                    hubConnection.Dispose();
              }).Start();
        }


        public static void Start()
        {
            hubConnection = new HubConnection("http://snuskelabben.cloudapp.net/");
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
                    // Do more stuff here
                }
            });

        }


        private static void HubConnection_StateChanged(StateChange stateChange)
        {
            if (stateChange.NewState == ConnectionState.Connected)
            {
                hubProxy.Invoke("JoinSession", "testroom");
                var myDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
                myDelegate.InvokeOnMainThread(startGps);

            }
            if (stateChange.NewState == ConnectionState.Disconnected) return;
            if (stateChange.NewState == ConnectionState.Reconnecting) return;
            if (stateChange.NewState == ConnectionState.Connecting) return;
        }


        private static void OnSignalRMessage(string message)
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

