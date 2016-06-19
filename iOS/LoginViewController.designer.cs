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
	[Register ("LoginViewController")]
	partial class LoginViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton BtnLoginSubmit { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField InputPassword { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField InputUsername { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView LoginLoginView { get; set; }

		[Action ("BtnLoginSubmit_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void BtnLoginSubmit_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (BtnLoginSubmit != null) {
				BtnLoginSubmit.Dispose ();
				BtnLoginSubmit = null;
			}
			if (InputPassword != null) {
				InputPassword.Dispose ();
				InputPassword = null;
			}
			if (InputUsername != null) {
				InputUsername.Dispose ();
				InputUsername = null;
			}
			if (LoginLoginView != null) {
				LoginLoginView.Dispose ();
				LoginLoginView = null;
			}
		}
	}
}
