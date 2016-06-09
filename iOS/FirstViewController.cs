using System;
using Common.Models;
using CoreGraphics;
using CoreLocation;
using Foundation;
using Google.Maps;
using UIKit;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;

namespace iOS
{
    public partial class FirstViewController : UIViewController
    {
        private MapView mapView;
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
            

            mapView.AddObserver(this, new NSString("myLocation"), NSKeyValueObservingOptions.New, IntPtr.Zero);
            View = mapView;
            //InvokeOnMainThread(() => mapView.MyLocationEnabled = true);
            
        }

        public override void ObserveValue(NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
        {
            //base.ObserveValue (keyPath, ofObject, change, context);

                var location = change.ObjectForKey(NSValue.ChangeNewKey) as CLLocation;
                Console.WriteLine(location.Coordinate.Longitude);
                Console.WriteLine(location.Coordinate.Latitude);
                mapView.Camera = CameraPosition.FromCamera(location.Coordinate, 14);
            
        }


    }
}