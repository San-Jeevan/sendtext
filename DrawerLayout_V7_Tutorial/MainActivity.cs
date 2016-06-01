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
	[Activity (Label = "DrawerLayout_V7_Tutorial", MainLauncher = false, Icon = "@drawable/icon", Theme="@style/MyTheme")]
	public class MainActivity : AppCompatActivity
    {
		private SupportToolbar mToolbar;
		private MyActionBarDrawerToggle mDrawerToggle;
		private DrawerLayout mDrawerLayout;
		private ListView mLeftDrawer;
		private ArrayAdapter mLeftAdapter;
		private List<string> mLeftDataSet;

        private ViewFlipper _flipper;
        private Button btnPrev, btnNext;


	    protected override void OnPause()
	    {
            StopService(new Intent(this, typeof(GPSservice)));
            base.OnPause();
	    }

	    void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Fragment fragment = null;
            var item = mLeftDataSet[e.Position];
            if (item == "Home")
            {
                fragment = new HomeFragment();
            }
            if (item == "Session")
            {
                fragment = new MapFragment();
            }
            if (item == "Settings")
            {
                fragment = new MapFragment();
            }
            if (item == "About")
            {
                fragment = new MapFragment();
            }

           FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, fragment).Commit();
           mDrawerLayout.CloseDrawer(mLeftDrawer);
        }
        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, new HomeFragment()).Commit();

            mToolbar = FindViewById<SupportToolbar> (Resource.Id.toolbar);
           
			mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
			mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);
            mLeftDrawer.ItemClick += listView_ItemClick;


            mLeftDrawer.Tag = 0;

            SetSupportActionBar(mToolbar);

            mLeftDataSet = new List<string>();
			mLeftDataSet.Add ("Home");
			mLeftDataSet.Add ("Session");
			mLeftDataSet.Add ("Settings");
			mLeftDataSet.Add ("About");
			mLeftAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, mLeftDataSet);
			mLeftDrawer.Adapter = mLeftAdapter;


			mDrawerToggle = new MyActionBarDrawerToggle(
				this,							//Host Activity
				mDrawerLayout,					//DrawerLayout
				Resource.String.openDrawer,		//Opened Message
				Resource.String.closeDrawer		//Closed Message
			);

            mDrawerLayout.AddDrawerListener(mDrawerToggle);
            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(true);
            mToolbar.SetNavigationIcon(Resource.Drawable.abc_ic_menu_moreoverflow_mtrl_alpha);
            mDrawerToggle.SyncState();

            if (bundle != null)
			{
				if (bundle.GetString("DrawerState") == "Opened")
				{
					SupportActionBar.SetTitle(Resource.String.openDrawer);
				}

				else
				{
					SupportActionBar.SetTitle(Resource.String.closeDrawer);
				}
			}

			else
			{
				//This is the first the time the activity is ran
				SupportActionBar.SetTitle(Resource.String.closeDrawer);
			}
		}
			
		public override bool OnOptionsItemSelected (IMenuItem item)
		{		
			switch (item.ItemId)
			{

			case Android.Resource.Id.Home:
				//The hamburger icon was clicked which means the drawer toggle will handle the event
				//all we need to do is ensure the right drawer is closed so the don't overlap
				mDrawerToggle.OnOptionsItemSelected(item);
				return true;

			case Resource.Id.action_refresh:
				//Refresh
				return true;

			default:
				return base.OnOptionsItemSelected (item);
			}
		}
			
		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.action_menu, menu);
			return base.OnCreateOptionsMenu (menu);
		}

		protected override void OnSaveInstanceState (Bundle outState)
		{
			if (mDrawerLayout.IsDrawerOpen((int)GravityFlags.Left))
			{
				outState.PutString("DrawerState", "Opened");
			}

			else
			{
				outState.PutString("DrawerState", "Closed");
			}

			base.OnSaveInstanceState (outState);
		}

		protected override void OnPostCreate (Bundle savedInstanceState)
		{
			base.OnPostCreate (savedInstanceState);
			mDrawerToggle.SyncState();
		}

		public override void OnConfigurationChanged (Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged (newConfig);
			mDrawerToggle.OnConfigurationChanged(newConfig);
		}
	}
}


