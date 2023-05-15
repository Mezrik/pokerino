using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Pokerino.Server.Helpers;
using Pokerino.Shared.Entities;
using Pokerino.Shared.Models.Rooms;

namespace Pokerino.Server.Services
{
    public interface IRoomService
    {
        Room CreateRoom(RoomCreateRequest model, User? user);
        JoinRoomResponse JoinRoom(string publicId, User? user, string? username);
        bool CheckIfRoomExists(string publicId);
        Room StartRoom(int roomId);
        Room RemoveUserFromRoom(int roomId, RoomUser user);
        List<Room> GetAllRoomsForUser(int userId);
        Room CreateRoomTopic(TopicCreateRequest model, int roomId);
        Room DeleteRoomTopic(int roomId, int topicId);
        Room UpdateRoomTopic(TopicUpdateRequest model, int roomId);
        Room SelectRoomTopic(int roomId, int topicId);
        Room SetRoomTopicEstimate(int roomId, int topicId, double estimate);
        Room VoteForRoomTopicEstimate(int roomId, int topicId, int estimate, string username);
        Room RestartRoomTopicVoting(int roomId, int topicId);
        Room ToggleTopicEstimateVotesVisibility(int roomId, int topicId);
    }

    public class RoomService : IRoomService
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public RoomService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Room CreateRoom(RoomCreateRequest model, User? user)
        {
            if (_context.Rooms.Any(x => x.PublicId == model.PublicId))
                throw new AppException("Room with the public Id '" + model.PublicId + "' already exists");

            var room = _mapper.Map<Room>(model);
            room.RoomUsers = new List<RoomUser>();

            var host = new RoomUser() { Role = Role.Host, User = user, Name = user?.Username ?? model.Username ?? "Host" };

            room.RoomUsers.Add(host);

            _context.Rooms.Add(room);
            _context.SaveChanges();

            return room;
        }

        public JoinRoomResponse JoinRoom(string publicId, User? user, string? username)
        {
            var room = GetRoomByPublicId(publicId);

            if (user is not null || username is not null)
            {
                var existingGuest = _context.Rooms
                    .FirstOrDefault(x => x.PublicId == publicId)
                    ?.RoomUsers
                    .FirstOrDefault(x => (user != null && x.User?.Id == user?.Id) || x.Name == username);

                if (existingGuest is not null)
                    return new() { Room = room, RoomUser = existingGuest };
            }

            var guest = new RoomUser() { Role = Role.Guest, User = user, Name = user?.Username ?? username };

            room.RoomUsers.Add(guest);

            _context.Rooms.Update(room);
            _context.SaveChanges();

            return new() { Room = room, RoomUser = guest };
        }

        public bool CheckIfRoomExists(string publicId)
        {
            return GetRoomByPublicId(publicId) is not null;
        }

        public Room StartRoom(int roomId)
        {
            var room = GetRoom(roomId);
            room.Status = RoomStatus.InProgress;

            _context.Rooms.Update(room);
            _context.SaveChanges();

            return room;
        }

        public Room RemoveUserFromRoom(int roomId, RoomUser user)
        {
            var room = GetRoom(roomId);

            if (user.Role == Role.Host) return room;

            room.RoomUsers = room.RoomUsers.Where(x => x.Id != user.Id).ToList();
            _context.Rooms.Update(room);
            _context.SaveChanges();

            return room;
        }

        public List<Room> GetAllRoomsForUser(int userId)
        {
            return _context.Rooms.Include(r => r.RoomUsers).Where(x => x.RoomUsers.Select(x => x.User.Id).Contains(userId)).ToList();
        }

        public Room CreateRoomTopic(TopicCreateRequest model, int roomId)
        {
            var room = GetRoom(roomId);
            var topic = _mapper.Map<RoomTopic>(model);

            room.Topics.Add(topic);

            _context.Rooms.Update(room);
            _context.SaveChanges();

            return room;
        }

        public Room UpdateRoomTopic(TopicUpdateRequest model, int roomId)
        {
            var room = GetRoom(roomId);
            var topic = room.Topics.FirstOrDefault(x => x.Id == model.Id);

            if (topic is null)
                throw new AppException("Topic with the Id '" + model.Id + "' does not exist");

            topic.Name = model.Name;
            topic.Description = model.Description;

            _context.Rooms.Update(room);
            _context.SaveChanges();

            return room;
        }

        public Room DeleteRoomTopic(int roomId, int topicId)
        {
            var room = GetRoom(roomId);

            room.Topics = room.Topics.Where(x => x.Id != topicId).ToList();

            _context.Rooms.Update(room);
            _context.SaveChanges();

            return room;
        }

        public Room SelectRoomTopic(int roomId, int topicId)
        {
            var room = GetRoom(roomId);

            room.ActiveTopic = room.Topics.FirstOrDefault(x => x.Id == topicId);

            _context.Rooms.Update(room);
            _context.SaveChanges();

            return room;
        }

        public Room SetRoomTopicEstimate(int roomId, int topicId, double estimate)
        {
            var room = GetRoom(roomId);

            var topic = room.Topics.FirstOrDefault(x => x.Id == topicId);

            if (topic is not null)
                topic.Estimate = estimate;

            _context.Rooms.Update(room);
            _context.SaveChanges();

            return room;
        }

        public Room VoteForRoomTopicEstimate(int roomId, int topicId, int estimate, string username)
        {
            var room = GetRoom(roomId);

            var topic = room.Topics.FirstOrDefault(x => x.Id == topicId);

            if (topic is null)
                throw new AppException("Topic with the Id '" + topicId + "' does not exist");

            if (topic.Votes is not null && topic.Votes.Any(x => x.Username == username))
                throw new AppException("User '" + username + "' already voted");

            if (topic.Votes is null) topic.Votes = new List<EstimateVote>();

            topic.Votes.Add(new EstimateVote() { Estimate = estimate, Username = username });

            _context.Rooms.Update(room);
            _context.SaveChanges();

            return room;
        }

        public Room RestartRoomTopicVoting(int roomId, int topicId)
        {
            var room = GetRoom(roomId);

            var topic = room.Topics.FirstOrDefault(x => x.Id == topicId);

            if (topic is null)
                throw new AppException("Topic with the Id '" + topicId + "' does not exist");


            topic.Votes = new List<EstimateVote>();
            topic.ShowVotes = false;

            _context.Rooms.Update(room);
            _context.SaveChanges();

            return room;
        }

        public Room ToggleTopicEstimateVotesVisibility(int roomId, int topicId)
        {
            var room = GetRoom(roomId);

            var topic = room.Topics.FirstOrDefault(x => x.Id == topicId);

            if (topic is null)
                throw new AppException("Topic with the Id '" + topicId + "' does not exist");

            topic.ShowVotes = !topic.ShowVotes;

            _context.Rooms.Update(room);
            _context.SaveChanges();

            return room;
        }


        private Room GetRoom(int roomId)
        {
            var room = _context.Rooms.Include(r => r.RoomUsers).Include(r => r.Topics).ThenInclude(t => t.Votes).FirstOrDefault(x => x.Id == roomId);
            if (room == null) throw new KeyNotFoundException("Room not found");
            return room;
        }

        private Room GetRoomByPublicId(string publicId)
        {
            var room = _context.Rooms.Include(r => r.RoomUsers).Include(r => r.Topics).ThenInclude(t => t.Votes).FirstOrDefault(x => x.PublicId == publicId);
            if (room == null) throw new KeyNotFoundException("Room not found");
            return room;
        }
    }
}

