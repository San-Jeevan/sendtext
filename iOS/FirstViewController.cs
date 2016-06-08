using System;
using CoreGraphics;
using Google.Maps;
using UIKit;
using Microsoft.AspNet.SignalR.Client;

namespace iOS
{
    public partial class FirstViewController : UIViewController
    {
        private MapView mapView;
        HubConnection hubConnection;
        IHubProxy hubProxy;
        public FirstViewController(IntPtr handle) : base(handle)
        {
        }

        public override void LoadView()
        {
            base.LoadView();
            var camera = CameraPosition.FromCamera(latitude: 37.79,
                                            longitude: -122.40,
                                            zoom: 6);
            mapView = MapView.FromCamera(CGRect.Empty, camera);
            mapView.MyLocationEnabled = true;
            View = mapView;

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

                }
            });



        }

        private void OnSignalRMessage(string message)
        {
           

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

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}