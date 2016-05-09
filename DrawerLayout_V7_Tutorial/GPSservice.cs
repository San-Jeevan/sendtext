using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Microsoft.AspNet.SignalR.Client;

namespace DrawerLayout_V7_Tutorial
{
    [Service]
    public class GPSservice : Service, GoogleApiClient.IConnectionCallbacks,
        GoogleApiClient.IOnConnectionFailedListener, Android.Gms.Location.ILocationListener
    {

        GoogleApiClient apiClient;
        LocationRequest locRequest;
        HubConnection hubConnection;
        IHubProxy hubProxy;
        bool _isGooglePlayServicesInstalled;


        public void startGps()
        {         
            // Setting location priority to PRIORITY_HIGH_ACCURACY (100)
            locRequest.SetPriority(100);

            // Setting interval between updates, in milliseconds
            // NOTE: the default FastestInterval is 1 minute. If you want to receive location updates more than 
            // once a minute, you _must_ also change the FastestInterval to be less than or equal to your Interval
            locRequest.SetFastestInterval(500);
            locRequest.SetInterval(1000);

            Log.Debug("LocationRequest", "Request priority set to status code {0}, interval set to {1} ms",
                locRequest.Priority.ToString(), locRequest.Interval.ToString());

            // pass in a location request and LocationListener
            LocationServices.FusedLocationApi.RequestLocationUpdates(apiClient, locRequest, this);

        }

        public override void OnDestroy()
        {
            if (apiClient.IsConnected)
            {
                LocationServices.FusedLocationApi.RemoveLocationUpdates(apiClient, this);
                apiClient.Disconnect();
            }
            base.OnDestroy();
        }

        public override bool OnUnbind(Intent intent)
        {
            if (apiClient.IsConnected)
            {
                LocationServices.FusedLocationApi.RemoveLocationUpdates(apiClient, this);
                apiClient.Disconnect();
            }
            return base.OnUnbind(intent);
        }


        private void SendLocationBroadcast(Intent intent, double latitude, double longitude)
        {
            intent.PutExtra("latitude", latitude);
            intent.PutExtra("longitude", longitude);
            LocalBroadcastManager.GetInstance(this).SendBroadcast(intent);
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {


            hubConnection = new HubConnection("http://snuskelabben.cloudapp.net:8022");
            hubProxy = hubConnection.CreateHubProxy("GpsHub");
            hubProxy.On<string>("UpdateChatMessage", OnSignalRMessage);
            hubConnection.StateChanged += HubConnection_StateChanged;
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

            hubConnection.Error += ex => Console.WriteLine("An error occurred {0}", ex.Message);

            hubConnection.Closed += () => Console.WriteLine("Connection with client id {0} closed", hubConnection.ConnectionId);


            //_isGooglePlayServicesInstalled = IsGooglePlayServicesInstalled();

            //if (_isGooglePlayServicesInstalled)
            //{
            //    // pass in the Context, ConnectionListener and ConnectionFailedListener
            //    apiClient = new GoogleApiClient.Builder(this, this, this)
            //        .AddApi(LocationServices.API).Build();

            //    // generate a location request that we will pass into a call for location updates
            //    locRequest = new LocationRequest();
            //    apiClient.Connect();

            //    //hent socket1.gpsfix.io
            //    //hent socket2.gpsfix.io
            //    //check health



            //}
            //else {
            //    Log.Error("OnCreate", "Google Play Services is not installed");
            //    Toast.MakeText(this, "Google Play Services is not installed", ToastLength.Long).Show();
            //}
            //#pragma warning disable 612, 618
            return base.OnStartCommand(intent, flags, startId);
            #pragma warning restore 612, 618
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

        public override IBinder OnBind(Intent intent)
        {
           
            return null;
        }


        bool IsGooglePlayServicesInstalled()
        {
            int queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (queryResult == ConnectionResult.Success)
            {
                Log.Info("MainActivity", "Google Play Services is installed on this device.");
                return true;
            }

            if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
            {
                string errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
                Log.Error("ManActivity", "There is a problem with Google Play Services on this device: {0} - {1}", queryResult, errorString);

                // Show error dialog to let user debug google play services
            }
            return false;
        }

        public void OnConnected(Bundle connectionHint)
        {
            Toast.MakeText(this, "Now connected to client", ToastLength.Short).Show();
            Log.Info("LocationClient", "Now connected to client");
            startGps();
        }

        public void OnConnectionSuspended(int cause)
        {
          
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            // This method is used to handle connection issues with the Google Play Services Client (LocationClient). 
            // You can check if the connection has a resolution (bundle.HasResolution) and attempt to resolve it

            // You must implement this to implement the IGooglePlayServicesClientOnConnectionFailedListener Interface
            Toast.MakeText(this, "Connection failed, attempting to reach google play services", ToastLength.Short).Show();
            Log.Info("LocationClient", "Connection failed, attempting to reach google play services");
        }

        public void OnLocationChanged(Location location)
        {
            Toast.MakeText(this, location.Latitude + ", " + location.Longitude, ToastLength.Short).Show();
            Log.Debug("LocationClient", "Location updated");
            Intent intent = new Intent("PosUpdate");
            SendLocationBroadcast(intent, location.Latitude, location.Longitude);
            SendLocationToInternet(intent, location.Latitude, location.Longitude);
        }


        private void OnSignalRMessage(string message)
        {
            
        }
        private void SendLocationToInternet(Intent intent, double latitude, double longitude)
        {
            if (hubConnection.State == ConnectionState.Connected)
            {
                hubProxy.Invoke("SendSessionMessage", latitude);
            }


        }
    }
}