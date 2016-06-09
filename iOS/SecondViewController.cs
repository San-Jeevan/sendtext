using System;
using System.Globalization;
using CoreLocation;
using Foundation;
using UIKit;

namespace iOS
{
    public partial class SecondViewController : UIViewController
    {
        private CLLocationManager locMgr2 = null;
        public SecondViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
  
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            var locMgr = new CLLocationManager();
            locMgr.PausesLocationUpdatesAutomatically = false;

            // handle the updated location method and update the UI
            if (UIDevice.CurrentDevice.CheckSystemVersion(6, 0))
            {
                locMgr.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
                {
                    var l = e.Locations[e.Locations.Length - 1];
                    this.LblLatitude.Text = l.Coordinate.Latitude.ToString(CultureInfo.InvariantCulture);
                    this.LblLongitude.Text = l.Coordinate.Longitude.ToString(CultureInfo.InvariantCulture);

                };
            }
            else
            {
#pragma warning disable 618
                // this won't be called on iOS 6 (deprecated)
                locMgr.UpdatedLocation += (object sender, CLLocationUpdatedEventArgs e) =>
                {
                    var l = e.NewLocation;
                    this.LblLatitude.Text = l.Coordinate.Latitude.ToString(CultureInfo.InvariantCulture);
                    this.LblLongitude.Text = l.Coordinate.Longitude.ToString(CultureInfo.InvariantCulture);

                };
#pragma warning restore 618
            }

            //iOS 8 requires you to manually request authorization now - Note the Info.plist file has a new key called requestWhenInUseAuthorization added to.
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                locMgr.RequestWhenInUseAuthorization();
            }


            // start updating our location, et. al.
            if (CLLocationManager.LocationServicesEnabled)
                locMgr.StartUpdatingLocation();

            //if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            //{
            //    locMgr.RequestAlwaysAuthorization();
            //}



            //if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            //{
            //    locMgr.AllowsBackgroundLocationUpdates = true;
            //}

            //locMgr.Failed += delegate (object sender, NSErrorEventArgs args)
            //{
            //    this.LblLatitude.Text = "FAILED";
            //};

            //locMgr.LocationUpdatesPaused += delegate (object sender, EventArgs args)
            //{
            //    this.LblLatitude.Text = "PAUSED";
            //};

            //if (CLLocationManager.LocationServicesEnabled)
            //{
            //    locMgr.DesiredAccuracy = CLLocation.AccuracyBest;
            //    locMgr.DistanceFilter = 1;

            //    locMgr.LocationsUpdated += delegate (object sender, CLLocationsUpdatedEventArgs e)
            //    {
            //        var l = e.Locations[e.Locations.Length - 1];
            //        this.LblLatitude.Text = l.Coordinate.Latitude.ToString(CultureInfo.InvariantCulture);
            //        this.LblLongitude.Text = l.Coordinate.Longitude.ToString(CultureInfo.InvariantCulture);

            //    };


            //}
            //locMgr.StartUpdatingLocation();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

     
    }
}

