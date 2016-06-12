using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using UIKit;

namespace iOS.APN
{
 
    public class APNService
    {
        private string APN_sandbox = "gateway.sandbox.push.apple.com";
        private string APN_production = "gateway.push.apple.com";

        public void GetApnToken()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                                   UIUserNotificationType.Alert,
                                   new NSSet());


                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }

        }

    }
}
