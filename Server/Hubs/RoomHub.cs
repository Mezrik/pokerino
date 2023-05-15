using System;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Pokerino.Server.Helpers;
using Pokerino.Server.Services;
using Pokerino.Shared.Entities;
using Pokerino.Shared.Models.Rooms;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Pokerino.Server.Hubs
{
    [AllowAnonymous]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class RoomHub : Hub
    {
        private readonly static ConnectionMapping<JoinRoomResponse> _connections =
            new ConnectionMapping<JoinRoomResponse>();

        private readonly IUserResolverService _userResolverService;
        private readonly IRoomService _roomService;

        public RoomHub(IUserResolverService userResolverService, IRoomService roomService)
        {
            _userResolverService = userResolverService;
            _roomService = roomService;
        }

        public async Task JoinRoom(string publicId, string? username)
        {
            var user = Context.User != null ? _userResolverService.GetUser(Context.User) : null;
            var room = _roomService.JoinRoom(publicId, user, username);

            await Groups.AddToGroupAsync(Context.ConnectionId, publicId);

            if (room is not null)
                _connections.Add(Context.ConnectionId, room);

            await Clients.Client(Context.ConnectionId).SendAsync("Joined", JsonSerializer.Serialize(room));

            await NotifyAboutRoomChange(room?.Room);
        }

        public async Task StartRoom()
        {
            var connData = _connections.GetConnectionData(Context.ConnectionId);

            if (connData is null) return;

            var room = _roomService.StartRoom(connData.Room.Id);

            await NotifyAboutRoomChange(room);
        }

        public async Task AddRoomTopic(TopicCreateRequest model)
        {
            var connData = _connections.GetConnectionData(Context.ConnectionId);

            if (connData is null) return;

            var room = _roomService.CreateRoomTopic(model, connData.Room.Id);

            await NotifyAboutRoomChange(room);
        }

        public async Task UpdateRoomTopic(TopicUpdateRequest model)
        {
            var connData = _connections.GetConnectionData(Context.ConnectionId);

            if (connData is null) return;

            var room = _roomService.UpdateRoomTopic(model, connData.Room.Id);

            await NotifyAboutRoomChange(room);
        }

        public async Task RemoveRoomTopic(int topicId)
        {
            var connData = _connections.GetConnectionData(Context.ConnectionId);

            if (connData is null) return;

            var room = _roomService.DeleteRoomTopic(connData.Room.Id, topicId);

            await NotifyAboutRoomChange(room);
        }

        public async Task SelectRoomTopic(int topicId)
        {
            var connData = _connections.GetConnectionData(Context.ConnectionId);

            if (connData is null) return;

            var room = _roomService.SelectRoomTopic(connData.Room.Id, topicId);

            await NotifyAboutRoomChange(room);
        }

        public async Task SelectFinalRoomTopicEstimate(int topicId, double estimate)
        {
            var connData = _connections.GetConnectionData(Context.ConnectionId);
            if (connData is null) return;

            var room = _roomService.SetRoomTopicEstimate(connData.Room.Id, topicId, estimate);
            await NotifyAboutRoomChange(room);
        }

        public async Task VoteForRoomTopicEstimate(int topicId, int estimate)
        {
            var connData = _connections.GetConnectionData(Context.ConnectionId);
            if (connData is null) return;

            var room = _roomService.VoteForRoomTopicEstimate(connData.Room.Id, topicId, estimate, connData.RoomUser.Name ?? "");
            await NotifyAboutRoomChange(room);
        }

        public async Task ToggleTopicEstimateVotesVisibility(int topicId)
        {
            var connData = _connections.GetConnectionData(Context.ConnectionId);
            if (connData is null) return;

            var room = _roomService.ToggleTopicEstimateVotesVisibility(connData.Room.Id, topicId);
            await NotifyAboutRoomChange(room);
        }

        public async Task RestartRoomTopicVoting(int topicId)
        {
            var connData = _connections.GetConnectionData(Context.ConnectionId);
            if (connData is null) return;

            var room = _roomService.RestartRoomTopicVoting(connData.Room.Id, topicId);
            await NotifyAboutRoomChange(room);
        }

        private async Task NotifyAboutRoomChange(Room? room)
        {
            if (room is null) return;

            await Clients.Groups(room.PublicId).SendAsync("RoomStateChange", JsonSerializer.Serialize(room));
        }

        //public override async Task OnConnectedAsync()
        //{
        //    string name = Context.User.Identity.Name;

        //    _connections.Add(name, Context.ConnectionId);

        //    await base.OnConnectedAsync();
        //}

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connData = _connections.GetConnectionData(Context.ConnectionId);

            if (connData is not null)
            {
                var room = _roomService.RemoveUserFromRoom(connData.Room.Id, connData.RoomUser);
                await NotifyAboutRoomChange(room);
            }

            await base.OnDisconnectedAsync(exception);
        }

        //public override Task OnReconnected()
        //{
        //    string name = Context.User.Identity.Name;

        //    if (!_connections.GetConnections(name).Contains(Context.ConnectionId))
        //    {
        //        _connections.Add(name, Context.ConnectionId);
        //    }

        //    return base.OnReconnected();
        //}
    }
}

