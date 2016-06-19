using System;
using CoreLocation;
using Foundation;
using UIKit;
using Google.Maps;
using iOS.APN;
using iOS.REST;


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
        private APNService apnService = null;

       
        public override UIWindow Window
        {
            get;
            set;
        }



        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {

            //CHECK IF USER CLICKED ON A NOTIFCATION.
            if (launchOptions != null)
            {
                if (launchOptions.ContainsKey(UIApplication.LaunchOptionsLocalNotificationKey))
                {
                    Console.WriteLine("local");
                    UILocalNotification localNotification = launchOptions[UIApplication.LaunchOptionsLocalNotificationKey] as UILocalNotification;
                    if (localNotification != null)
                    {
                        new UIAlertView(localNotification.AlertAction, localNotification.AlertBody, null, "OK", null).Show();
                    }
                }

             
                if (launchOptions.ContainsKey(UIApplication.LaunchOptionsRemoteNotificationKey))
                {
                    NSDictionary remoteNotification = launchOptions[UIApplication.LaunchOptionsRemoteNotificationKey] as NSDictionary;
                    if (remoteNotification != null)
                    {
                        //new UIAlertView(remoteNotification.AlertAction, remoteNotification.AlertBody, null, "OK", null).Show();
                    }
                }
            }

            //APN
            apnService = new APNService();
            apnService.GetApnToken();


            //NAVIGATION WHITE
            UINavigationBar.Appearance.TintColor = UIColor.White;
            UINavigationBar.Appearance.BarTintColor = UIColor.Purple;
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes() { TextColor = UIColor.White });
            MapServices.ProvideAPIKey(mapskey);


            //LOAD NavigationCreateController AS FIRSTVIEW
            Window = new UIWindow(UIScreen.MainScreen.Bounds)
            {
                RootViewController = new NavigationCreateController()
            };
            Window.MakeKeyAndVisible();

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
            GpsService.Destroy();
            //_gpsService = null;
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
            //GpsService.Start();
            //}
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            GpsService.Destroy();
            //_gpsService = null;
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            var DeviceToken = deviceToken.Description;
            Console.WriteLine(DeviceToken);
            if (!string.IsNullOrWhiteSpace(DeviceToken))
            {
                DeviceToken = DeviceToken.Trim('<').Trim('>');
            }

            var oldDeviceToken = NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken");

            if (string.IsNullOrEmpty(oldDeviceToken) || !oldDeviceToken.Equals(DeviceToken))
            {
            }

            // Save new device token 
            NSUserDefaults.StandardUserDefaults.SetString(DeviceToken, "PushDeviceToken");
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {

            new UIAlertView("Error registering push notifications", error.LocalizedDescription, null, "OK", null).Show();
        }
        

        public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
        {
            // show an alert
            Console.WriteLine("ReceivedLocalNotification");
            new UIAlertView(notification.AlertAction, notification.AlertBody, null, "OK", null).Show();

          
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            new UIAlertView("ok", "userInfo", null, "OK", null).Show();
            Console.WriteLine("ReceivedRemoteNotification");
            // reset our badge
        }
    }
}