using System;
using Common.Models;
using CoreGraphics;
using CoreLocation;
using FlyoutNavigation;
using Foundation;
using Google.Maps;
using UIKit;
using Microsoft.AspNet.SignalR.Client;
using MonoTouch.Dialog;
using Newtonsoft.Json;

namespace iOS
{
    public partial class FirstViewController : UIViewController
    {
        private MapView mapView;
        private FlyoutNavigationController flyoutcontroller;
     public FirstViewController(IntPtr handle) : base(handle)
        {
        }


        public void setNavigation(FlyoutNavigationController flyout)
        {
            flyoutcontroller = flyout;
        }

        public override void LoadView()
        {
            base.LoadView();
            var camera = CameraPosition.FromCamera(latitude: 37.79,
                                            longitude: -122.40,
                                            zoom: 6);

           
            mapView = MapView.FromCamera(CGRect.Empty, camera);
            View = mapView;
           InvokeOnMainThread(() => mapView.MyLocationEnabled = true);
            
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationItem.Title = "Session initiated";
        
            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIImage.FromFile("Menu-20.png"),
    UIBarButtonItemStyle.Plain, delegate {
        flyoutcontroller.ToggleMenu();
            });
        }

        public override void ObserveValue(NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
        {
                var location = change.ObjectForKey(NSValue.ChangeNewKey) as CLLocation;
                Console.WriteLine(location.Coordinate.Longitude);
                Console.WriteLine(location.Coordinate.Latitude);
                mapView.Camera = CameraPosition.FromCamera(location.Coordinate, 14);
        }


    }
}