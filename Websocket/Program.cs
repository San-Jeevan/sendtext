using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;

namespace Websocket
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://*:8022";
            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
            }
        }
    }
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }


    public class GpsHub : Hub
    {
        public override Task OnDisconnected(bool stopCalled)
        {
            Groups.Remove(Context.ConnectionId, "");
            return base.OnDisconnected(stopCalled);
        }

        public Task JoinSession(string sessionName)
        {
            Console.WriteLine("Joining {0}", sessionName);
            return Groups.Add(Context.ConnectionId, sessionName);
        }

        public Task LeaveSession(string sessionName)
        {
            Console.WriteLine("Leaving {0}", sessionName);
            return Groups.Remove(Context.ConnectionId, sessionName);
        }

      



        public void SendSessionMessage(string sessionName, string message)
        {
            Console.WriteLine("SendSessionMessage {0}, {1}", sessionName, message);
            var caller = Context.ConnectionId;
            Clients.Group(sessionName).locationUpdate(message);
        }
    }
}
