using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace Common.Credentials
{
    public static class SecureStore
    {

        static string AppNameVar = "gpsfix.io";

        //ANDROID
        public static void SaveCredentialsAndroid(string userName, string password)
        {
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
            {
                Account account = new Account
                {
                    Username = userName
                };
                account.Properties.Add("Password", password);
                account.Properties.Add("Token", "");
                AccountStore.Create().Save(account, AppNameVar);
            }
        }

        public static void SaveTokenAndroid(string userName, string token)
        {
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(token))
            {
                var account = AccountStore.Create().FindAccountsForService(AppNameVar).FirstOrDefault();
                if (account.Username != null)
                {
                    account.Properties["Token"] = token;
                }
            }
        }

        public static string GetUserNameAndroid
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(AppNameVar).FirstOrDefault();
                return (account != null) ? account.Username : null;
            }
        }

        public static string GetPasswordAndroid
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(AppNameVar).FirstOrDefault();
                return (account != null) ? account.Properties["Password"] : null;
            }
        }


        public static string GetTokenAndroid
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(AppNameVar).FirstOrDefault();
                return (account != null) ? account.Properties["Token"] : null;
            }
        }





        //IOS

        public static void SaveCredentialsIos(string userName, string password)
        {
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
            {
                Account account = new Account
                {
                    Username = userName
                };
                account.Properties.Add("Password", password);
                account.Properties.Add("Token", "");
                AccountStore.Create().Save(account, AppNameVar);
            }
        }

        public static void SaveTokenIos(string userName, string token)
        {
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(token))
            {
                var account = AccountStore.Create().FindAccountsForService(AppNameVar).FirstOrDefault();
                if (account.Username != null)
                {
                    account.Properties["Token"] = token;
                }
            }
        }

    }
}
