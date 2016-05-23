using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Provider;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace DrawerLayout_V7_Tutorial
{

    internal class HomeFragment : Fragment
    {


        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            var uri = ContactsContract.CommonDataKinds.Phone.ContentUri;
            string[] projection = {
   ContactsContract.Contacts.InterfaceConsts.Id,
   ContactsContract.Contacts.InterfaceConsts.DisplayName,
   ContactsContract.CommonDataKinds.Phone.Number
};
            var loader = new CursorLoader(this.Activity, uri, projection, null, null, null);
            var cursor = (ICursor)loader.LoadInBackground();


            cursor.MoveToFirst();
            do
            {
                var ContactId = cursor.GetLong(cursor.GetColumnIndex(projection[0]));
                var Name = cursor.GetString(cursor.GetColumnIndex(projection[1]));
                var number = cursor.GetString(cursor.GetColumnIndex(projection[2]));
                Log.Debug("smedrix", Name + " " + number);

            } while (cursor.MoveToNext());

            //var fromColumns = new string[] { ContactsContract.Contacts.InterfaceConsts.DisplayName };
            //var toControlIds = new int[] { Android.Resource.Id.Text1 };
            //var adapter = new SimpleCursorAdapter(this.Activity, Android.Resource.Layout.SimpleListItem1, cursor, fromColumns, toControlsIds);
            //listView.Adapter = adapter;
            base.OnViewCreated(view, savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (container == null)
            {
                // Currently in a layout without a container, so no reason to create our view.
                return null;
            }

            return inflater.Inflate(Resource.Layout.HomeLayout, container, false);
        }


    }

}