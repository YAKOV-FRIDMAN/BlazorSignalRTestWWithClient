using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorSignalRTest.Shared;

namespace BlazorSignalRTest.Server.Hubs
{

    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, User> ChatClients = new ConcurrentDictionary<string, User>();
        public async Task SendMessage(string user, string toUser, string message)
        {
            string idUser = ChatClients.FirstOrDefault(_ => _.Value.Email == toUser).Key;
            await Clients.Client(idUser).SendAsync("ReceiveMessage", user, message);
            //await Clients.User(idUser).SendAsync("ReceiveMessage", user, message);
        }
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
        public async Task Login(string userInput)
        {
            var r = ChatClients.FirstOrDefault(_ => _.Value.Email == userInput);
            if (r.Key == null)
            {
                ChatClients.TryAdd(Context.ConnectionId, new User { Email = userInput , IsConected = true});
                await ReceiveUsersStart();
                //Conenct

                await Clients.Client(Context.ConnectionId).SendAsync("Conenct", "Conenct !!!");
            }
            //else if(r.Value.IsConected == false)
            //{
            //    r.Value.IsConected = true;
            //    await Clients.Client(Context.ConnectionId).SendAsync("Conenct", "Conenct !!!");
            //    await ReceiveUsersStart();
            //}
        }

        public async Task ReceiveUsersStart()
        {
            var r = ChatClients.FirstOrDefault(_ => _.Value.Email == "ControlSever").Key;
           // if (r == null)
                await Clients.Client(r).SendAsync("ReceiveUsers", ChatClients.Values.ToList());
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var r = ChatClients.FirstOrDefault(_ => _.Key == Context.ConnectionId);
            if (r.Key != null)
            {
                 ChatClients.TryRemove(r);
               //  r.Value.IsConected = false;
                await ReceiveUsersStart();
            }

            //return base.OnDisconnectedAsync(exception);
        }


    }
}
