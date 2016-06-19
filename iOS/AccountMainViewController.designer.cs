// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace iOS
{
	[Register ("AccountMainViewController")]
	partial class AccountMainViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView AccountMain { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton Btnlogin { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton BtnRegister { get; set; }

		[Action ("Btnlogin_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void Btnlogin_TouchUpInside (UIButton sender);

		[Action ("UIButton11_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void UIButton11_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (AccountMain != null) {
				AccountMain.Dispose ();
				AccountMain = null;
			}
			if (Btnlogin != null) {
				Btnlogin.Dispose ();
				Btnlogin = null;
			}
			if (BtnRegister != null) {
				BtnRegister.Dispose ();
				BtnRegister = null;
			}
		}
	}
}
