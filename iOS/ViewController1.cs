using System;
using UIKit;

namespace iOS
{
    class NavBarDelegate : UINavigationBarDelegate
    {
        public override UIBarPosition GetPositionForBar(IUIBarPositioning barPositioning)
        {
            return UIBarPosition.TopAttached;
        }
    }
    public partial class ViewController1 : UIViewController
    {
        public ViewController1(IntPtr handle) : base(handle)
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
            UINavigationItem item = new UINavigationItem();
            item.Title = "Login";
            UIBarButtonItem backbutton = new UIBarButtonItem("Cancel", UIBarButtonItemStyle.Done, CancelClicked);
            backbutton.TintColor = UIColor.White;
            item.LeftBarButtonItem = backbutton;
            Toolbar.PushNavigationItem(item, true);
            Toolbar.Delegate = new NavBarDelegate();
            Myop.BackgroundColor = UIColor.FromPatternImage(UIImage.FromFile("LoginBg.jpg"));
        }

        private void CancelClicked(object sender, EventArgs e)
        {
            DismissViewController(true, null);
        }
    }
}