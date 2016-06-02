using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace DrawerLayout_V7_Tutorial
{

    internal class LoginRegisterFragment : Fragment
    {
        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            Button LoginRegister = (Button)this.Activity.FindViewById(Resource.Id.BtnLgnRegister);
            TextView LoginBtn = (TextView)this.Activity.FindViewById(Resource.Id.BtnLogin);

            LoginBtn.Click += delegate
            {
                ReplaceFragment(LoginAction.Login);
            };

            LoginRegister.Click += delegate
            {
                //api code
            };

            base.OnViewCreated(view, savedInstanceState);
        }

        private void ReplaceFragment(LoginAction choice)
        {
            Fragment newFragment = null;
            if (choice == LoginAction.Login) newFragment = new LoginLoginFragment();
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

            return inflater.Inflate(Resource.Layout.LoginRegister, container, false);
        }


    }

}