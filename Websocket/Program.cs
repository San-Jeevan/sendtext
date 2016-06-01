using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Common.Models;
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

        public static Dictionary<string, string> Dictionary = new Dictionary<string, string>();

        public override Task OnDisconnected(bool stopCalled)
        {
            
            if (Dictionary.ContainsKey(Context.ConnectionId))
            {
                StatusUpdPacket statusUpdate = new StatusUpdPacket() {SignalRId = Context.ConnectionId, Description = "", Type = UpdateType.StatusUpdate, Status = StatusType.Disconnected};
                string sessionName = Dictionary[Context.ConnectionId];
                Dictionary.Remove(Context.ConnectionId);
                Groups.Remove(Context.ConnectionId, sessionName);
                Clients.Group(sessionName).LocationUpdate(statusUpdate.ToString());
            }

            return base.OnDisconnected(stopCalled);
        }

        public Task JoinSession(string sessionName)
        {
            Console.WriteLine("Joining {0}", sessionName);
            Dictionary.Add(Context.ConnectionId, sessionName);
            StatusUpdPacket statusUpdate = new StatusUpdPacket() { SignalRId = Context.ConnectionId, Description = "", Type = UpdateType.StatusUpdate, Status = StatusType.Connected };
            Clients.Group(sessionName).LocationUpdate(statusUpdate.ToString());
            return Groups.Add(Context.ConnectionId, sessionName);
        }

        public Task LeaveSession(string sessionName)
        {
            Console.WriteLine("Leaving {0}", sessionName);
            if (Dictionary.ContainsKey(Context.ConnectionId))
            {
                StatusUpdPacket statusUpdate = new StatusUpdPacket() { SignalRId = Context.ConnectionId, Description = "", Type = UpdateType.StatusUpdate, Status = StatusType.Disconnected };
                string sessionNameFromDic = Dictionary[Context.ConnectionId];
                Dictionary.Remove(Context.ConnectionId);
                Groups.Remove(Context.ConnectionId, sessionNameFromDic);
                Clients.Group(sessionNameFromDic).LocationUpdate(statusUpdate.ToString());
            }
            return Groups.Remove(Context.ConnectionId, sessionName);
        }


        public void SendSessionMessage(string sessionName, string message)
        {
            Console.WriteLine("SendSessionMessage {0}, {1}", sessionName, message);
          
            Clients.Group(sessionName).LocationUpdate(message);
        }
    }
}
