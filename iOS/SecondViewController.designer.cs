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
	[Register ("SecondViewController")]
	partial class SecondViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ConnectBtn { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel LblLatitude { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel LblLongitude { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (ConnectBtn != null) {
				ConnectBtn.Dispose ();
				ConnectBtn = null;
			}
			if (LblLatitude != null) {
				LblLatitude.Dispose ();
				LblLatitude = null;
			}
			if (LblLongitude != null) {
				LblLongitude.Dispose ();
				LblLongitude = null;
			}
		}
	}
}
