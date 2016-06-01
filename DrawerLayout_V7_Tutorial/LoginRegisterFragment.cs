using Android.App;
using Android.OS;
using Android.Views;

namespace DrawerLayout_V7_Tutorial
{

    internal class LoginRegisterFragment : Fragment
    {
        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
          base.OnViewCreated(view, savedInstanceState);
        }


        private void ReplaceFragment()
        {
            Fragment newFragment = new ExampleFragment();
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.logincontent_frame, newFragment);
            transaction.AddToBackStack(null);
            transaction.Commit();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (container == null)
            {
                // Currently in a layout without a container, so no reason to create our view.
                return null;
            }

            return inflater.Inflate(Resource.Layout.LoginChoice, container, false);
        }


    }

}