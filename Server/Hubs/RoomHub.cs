using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Pokerino.Server.Services;

namespace Pokerino.Server.Hubs
{
    public class RoomHub : Hub
    {
        private readonly IUserResolverService _userResolverService;
        private readonly IRoomService _roomService;

        public RoomHub(IUserResolverService userResolverService, IRoomService roomService)
        {
            _userResolverService = userResolverService;
            _roomService = roomService;
        }

        [Authorize]
        public async Task JoinRoom(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

            if (Context.User == null) return;

            var user = _userResolverService.GetUser(Context.User);

            await Clients.Group(roomName).SendAsync("Send", $"{user?.Username} has joined the group {roomName}.");
        }

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}

