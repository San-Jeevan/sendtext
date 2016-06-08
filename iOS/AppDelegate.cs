using System;
using CoreLocation;
using Foundation;
using UIKit;
using Google.Maps;



namespace iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        string mapskey = "AIzaSyD0DCGzTuwLDr2gDodOL5fhEE3tQJHmyGo";

        private GpsService _gpsService = null;
        public override UIWindow Window
        {
            get;
            set;
        }

        public class MyLocationDelegate : CLLocationManagerDelegate
        {
            public override void LocationsUpdated(CLLocationManager manager, CLLocation[] locations)
            {
                foreach (var loc in locations)
                {
                    Console.WriteLine(loc);
                }
            }
        }


        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method
            MapServices.ProvideAPIKey(mapskey);
            //_gpsService = new GpsService();
            //_gpsService.Start();


            var locMgr = new CLLocationManager();
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
                locMgr.DesiredAccuracy = CLLocation.AccuracyBest;
                locMgr.DistanceFilter = 1;
        
                locMgr.LocationsUpdated += delegate (object sender, CLLocationsUpdatedEventArgs e) {
                    foreach (CLLocation l in e.Locations)
                    {
                        Console.WriteLine(l.Coordinate.Latitude + ", " + l.Coordinate.Longitude);
                    }
                };
                locMgr.StartUpdatingLocation();
            }

            return true;
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            _gpsService.Destroy();
            _gpsService = null;
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }

        public override void OnActivated(UIApplication application)
        {
            if(_gpsService == null) { 
            _gpsService = new GpsService();
            _gpsService.Start();
            }
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            _gpsService.Destroy();
            _gpsService = null;
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }
    }
}