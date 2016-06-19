using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using CoreLocation;
using Foundation;
using UIKit;
using FlyoutNavigation;
using MonoTouch.Dialog;

namespace iOS
{
    public partial class NavigationCreateController : UIViewController
    {
        FlyoutNavigationController navigation;

        public static T CreateViewController<T>(string storyboardName, string viewControllerStoryBoardId = "") where T : UIViewController
        {
            var storyboard = UIStoryboard.FromName(storyboardName, null);
            return string.IsNullOrEmpty(viewControllerStoryBoardId) ? (T)storyboard.InstantiateInitialViewController() : (T)storyboard.InstantiateViewController(viewControllerStoryBoardId);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var storyboardVc = CreateViewController<MapViewController>("Main", "MapViewController");
            var navigation = new FlyoutNavigationController
            {
                NavigationRoot = new RootElement("Navigation") {
                new Section ("") {
                new StringElement ("Home"),
                new StringElement ("Session"),
                new StringElement ("Settings")
             }
             }
            };
             

            storyboardVc.SetNavigation(navigation);
            var viewcontrollers = new[]
            {
                new UINavigationController(storyboardVc),
                new UIViewController {View = new UILabel {Text = "Vegetables (drag right)"}}, //-skal peke til home hvor nav bar er skjult
                new UIViewController {View = new UILabel {Text = "Minerals (drag right)"}}
            };
            navigation.ViewControllers = viewcontrollers;

            // Specify navigation position
            navigation.HideMenu();
           
            navigation.Position = FlyOutNavigationPosition.Left;
            View.AddSubview(navigation.View);
        }

    }

}



