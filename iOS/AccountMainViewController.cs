using System;

using UIKit;

namespace iOS
{
    public partial class AccountMainViewController : UIViewController
    {
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

            // Perform any additional setup after loading the view, typically from a nib.
        }

        partial void UIButton11_TouchUpInside(UIButton sender)
        {
        }

        partial void Btnlogin_TouchUpInside(UIButton sender)
        {
        }
    }
}