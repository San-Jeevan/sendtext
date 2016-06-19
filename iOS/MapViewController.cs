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
    public partial class MapViewController : UIViewController
    {
        private MapView mapView;
        private FlyoutNavigationController navigation;


        public static T CreateViewController<T>(string storyboardName, string viewControllerStoryBoardId = "") where T : UIViewController
        {
            var storyboard = UIStoryboard.FromName(storyboardName, null);
            return string.IsNullOrEmpty(viewControllerStoryBoardId) ? (T)storyboard.InstantiateInitialViewController() : (T)storyboard.InstantiateViewController(viewControllerStoryBoardId);
        }

        public MapViewController(IntPtr handle) : base(handle)
        {
        }

        public void SetNavigation(FlyoutNavigationController mynavigation)
        {
            navigation = mynavigation;
        }



        public override void LoadView()
        {
            base.LoadView();
            var camera = CameraPosition.FromCamera(latitude: 37.79,
                                            longitude: -122.40,
                                            zoom: 6);


            mapView = MapView.FromCamera(CGRect.Empty, camera);
            View = mapView;
          

        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationItem.Title ="Add participant to start...";
            NavigationItem.LeftBarButtonItem = new UIBarButtonItem(UIImage.FromFile("Menu-20.png"),
            UIBarButtonItemStyle.Plain, delegate
            {
                navigation.ToggleMenu();
            });

            

            var rightbtn = new UIBarButtonItem(UIBarButtonSystemItem.Add, delegate
            {
               Console.WriteLine("clicked");

                var newVC = CreateViewController<ViewController1>("Login", "ViewController1");

                newVC.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
                newVC.ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
               
                PresentViewController(newVC, true, ModalShow);
            });
     

            NavigationItem.RightBarButtonItem = rightbtn;
        }

        private void ModalShow()
        {
           Console.WriteLine("modal shown");
        }
    }
}