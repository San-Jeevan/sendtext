using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace StressTest
{

    class Program
    {
        private static Random random = new Random((int)DateTime.Now.Ticks);
        private static string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        static void Main(string[] args)
        {
            var success = 0;
            var error = 0;

            var settings_delay = 10000;
            var settings_requests = 40;
            
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < settings_requests; j++)
                {
                    Task.Factory.StartNew(() =>
            {
               
            var hubConnection = new HubConnection("http://snuskelabben.cloudapp.net/");
            var hubProxy = hubConnection.CreateHubProxy("GpsHub");

            hubConnection.StateChanged += delegate(StateChange change)
            {
                if (change.NewState == ConnectionState.Connected)
                {
                    hubProxy.Invoke("JoinSession", RandomString(11));
                }
            };
            hubConnection.Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    error += 1;
                    Console.WriteLine(String.Format("Error:{0} Success {1}", error, success));
                }
                else
                {
                    success += 1;
                    Console.WriteLine(String.Format("Error:{0} Success {1}", error, success));
                }
            });

            });
                }

                Thread.Sleep(settings_delay);

            }
            Console.ReadLine();
        }


     

    }

}
