using System;
using Microsoft.AspNetCore.SignalR;

namespace Pokerino.Server.Hubs
{
    public class RoomHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}

