using System;

using UIKit;

namespace iOS
{
    public partial class AccountMainViewController : UIViewController
    {
        public static T CreateViewController<T>(string storyboardName, string viewControllerStoryBoardId = "") where T : UIViewController
        {
            var storyboard = UIStoryboard.FromName(storyboardName, null);
                        return string.IsNullOrEmpty(viewControllerStoryBoardId) ? (T)storyboard.InstantiateInitialViewController() : (T)storyboard.InstantiateViewController(viewControllerStoryBoardId);
                   }

        public AccountMainViewController(IntPtr handle) : base(handle)
        {
            
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            AccountMain.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile("LoginBg.jpg"));
          
        }


        partial void BtnRegister_TouchUpInside(UIButton sender)
        {
             var newVC = CreateViewController<AccountRegisterViewController>("Login", "AccountRegisterViewController");
           
            newVC.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
            newVC.ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
            
            PresentViewController(newVC, true, null);
        }

        partial void Btnlogin_TouchUpInside(UIButton sender)
        {
        }
    }
}