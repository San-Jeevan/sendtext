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
	[Register ("ViewController1")]
	partial class ViewController1
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView ContactsList { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView Myop { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISearchDisplayController searchDisplayController { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UINavigationBar Toolbar { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (ContactsList != null) {
				ContactsList.Dispose ();
				ContactsList = null;
			}
			if (Myop != null) {
				Myop.Dispose ();
				Myop = null;
			}
			if (searchDisplayController != null) {
				searchDisplayController.Dispose ();
				searchDisplayController = null;
			}
			if (Toolbar != null) {
				Toolbar.Dispose ();
				Toolbar = null;
			}
		}
	}
}
