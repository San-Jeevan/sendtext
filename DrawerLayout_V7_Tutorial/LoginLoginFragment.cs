using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace DrawerLayout_V7_Tutorial
{

    internal class LoginLoginFragment : Fragment
    {
        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            Button LoginBtn = (Button)this.Activity.FindViewById(Resource.Id.BtnLgnLgnLogin);
            TextView LoginRegister = (TextView)this.Activity.FindViewById(Resource.Id.BtnLgnLgnRegister);

            LoginBtn.Click += delegate
            {
                //code for registering in webapi
            };

            LoginRegister.Click += delegate
            {
                ReplaceFragment(LoginAction.Register);
            };


            base.OnViewCreated(view, savedInstanceState);
        }


        private void ReplaceFragment(LoginAction choice)
        {
            Fragment newFragment = null;
            if (choice == LoginAction.Register) newFragment = new LoginRegisterFragment();
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

            return inflater.Inflate(Resource.Layout.LoginLogin, container, false);
        }


    }

}