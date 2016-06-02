using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace DrawerLayout_V7_Tutorial
{

    public enum LoginAction { Guest, Register, Login };

    internal class LoginFragment : Fragment
    {
        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {

            Button LoginBtn = (Button)this.Activity.FindViewById(Resource.Id.BtnLgnChoiceLgn);
            Button LoginRegister = (Button)this.Activity.FindViewById(Resource.Id.BtnLgnChoiceRegister);
            Button LoginGuest = (Button)this.Activity.FindViewById(Resource.Id.BtnLgnChoiceGuest);

            LoginBtn.Click += delegate {
                ReplaceFragment(LoginAction.Login);
            };

            LoginRegister.Click += delegate {
                ReplaceFragment(LoginAction.Register);
            };


            LoginGuest.Click += delegate {
                StartHomeActivity();
            };
            base.OnViewCreated(view, savedInstanceState);
        }


        private void StartHomeActivity()
        {
            var activity2 = new Intent(this.Activity, typeof(MainActivity));
            activity2.PutExtra("Mode", "Guest");
            StartActivity(activity2);
        }

        private void ReplaceFragment(LoginAction choice)
        {
            Fragment newFragment = null;
            if (choice == LoginAction.Login) newFragment = new LoginLoginFragment();
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

            return inflater.Inflate(Resource.Layout.LoginChoice, container, false);
        }


    }

}