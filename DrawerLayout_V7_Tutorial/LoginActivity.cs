using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using System.Collections.Generic;
using Java.Lang;

namespace DrawerLayout_V7_Tutorial
{
	[Activity (Label = "GPSFIX Login", MainLauncher = true, Icon = "@drawable/icon", Theme="@style/MyTheme")]
	public class LoginActivity : AppCompatActivity
	{
		private SupportToolbar mToolbar;
		private MyActionBarDrawerToggle mDrawerToggle;
		private DrawerLayout mDrawerLayout;
		private ListView mLeftDrawer;
		private ArrayAdapter mLeftAdapter;
		private List<string> mLeftDataSet;

        private ViewFlipper _flipper;
        private Button btnPrev, btnNext;


	   

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Login);

            FragmentManager.BeginTransaction().Replace(Resource.Id.logincontent_frame, new LoginFragment()).Commit();

        }
	

		
		protected override void OnPostCreate (Bundle savedInstanceState)
		{
			base.OnPostCreate (savedInstanceState);
			
		}

		public override void OnConfigurationChanged (Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged (newConfig);
			
		}
	}
}


