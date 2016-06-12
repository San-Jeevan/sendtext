using System;
using System.Net.Http;
using System.Threading.Tasks;


namespace iOS.REST
{
    public static class RESTService
    {
        private static HttpClient Client = null;

        static RESTService()
        {
            if (Client == null)
            {
                Client = new HttpClient();
                Client.MaxResponseContentBufferSize = 256000;
            }
        }


        public static async Task<bool> UpdateApnToken(string newToken)
        {
            var uri = new Uri(string.Format("", string.Empty));

            var response = await Client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
            }
            return true;
        }


        public static async Task<bool> CheckIfAlreadyRegistered(string mobilePhone)
        {
            var uri = new Uri(string.Format("", string.Empty));
 
            var response = await Client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
            }
            return true;
        }

        public static async Task<bool> CreateGuestUser(string mobilePhone)
        {
            var uri = new Uri(string.Format("", string.Empty));

            var response = await Client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
            }
            return true;
        }

    }
}
