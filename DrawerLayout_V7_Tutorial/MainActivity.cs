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
using Android.Graphics;
using Android.Support.V4.Content;
using Java.Lang;

namespace DrawerLayout_V7_Tutorial
{
	[Activity (Label = "GPS Fix", MainLauncher = false, Theme="@style/MyTheme")]
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
	    private MapFragment mapFragment = null;


	    protected override void OnPause()
	    {
            //OrientChange --> Isfinishing = false
            if (IsFinishing) StopService(new Intent(this, typeof(GPSservice)));
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
            SetContentView(Resource.Layout.Main);
           
          
            string launchMode = Intent.GetStringExtra("Mode") ?? "Data not available";
            mToolbar = FindViewById<SupportToolbar> (Resource.Id.toolbar);
			mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
			mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);
            mLeftDrawer.ItemClick += listView_ItemClick;
            mLeftDrawer.Tag = 0;

            if (launchMode == "Guest")
            {
                mToolbar.SetBackgroundColor(new Android.Graphics.Color(ContextCompat.GetColor(this, Resource.Color.indigo)));
                mToolbar.SetTitle(Resource.String.guestMode);
            }

            SetSupportActionBar(mToolbar);
            mLeftDataSet = new List<string>();
			mLeftDataSet.Add ("Home");
			mLeftDataSet.Add ("History");
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
            mToolbar.SetNavigationIcon(Resource.Drawable.ic_menu);
            mDrawerToggle.SyncState();
           

        }
			
		public override bool OnOptionsItemSelected (IMenuItem item)
		{		
			switch (item.ItemId)
			{

			case Android.Resource.Id.Home:
				mDrawerToggle.OnOptionsItemSelected(item);
				return true;

			case Resource.Id.action_addparticipant:
			     mapFragment.AddParticipantBtnClicked();
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

	    protected override void OnResume()
	    {
	        mapFragment = new MapFragment();
            FragmentManager.BeginTransaction().Replace(Resource.Id.content_frame, mapFragment).Commit();
            base.OnResume();
	    }


	    public override void OnConfigurationChanged (Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged (newConfig);
			mDrawerToggle.OnConfigurationChanged(newConfig);
		}

	  
    }
}


