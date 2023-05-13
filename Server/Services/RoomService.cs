using System;
using AutoMapper;
using Microsoft.Extensions.Hosting;
using Pokerino.Server.Helpers;
using Pokerino.Shared.Entities;
using Pokerino.Shared.Models.Rooms;

namespace Pokerino.Server.Services
{
    public interface IRoomService
    {
        public Room CreateRoom(RoomCreateRequest model, User? User);
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

            var host = new RoomUser() { Role = Role.Host, User = user, Name = model.Username };

            if (host.Name is null)
            {
                host.Name = user?.Username ?? "Host";
            }

            room.Host = host;

            _context.RoomUser.Add(host);
            _context.SaveChanges();

            room.Users.Add(host);

            _context.Rooms.Add(room);
            _context.SaveChanges();

            return room;
        }

        public Room JoinRoom(string publicId, User? user)
        {
            var room = GetRoomByPublicId(publicId);

            room.Users.Add(new RoomUser() { Role = Role.Guest, User = user });

            _context.Rooms.Update(room);
            _context.SaveChanges();

            return room;
        }

        private Room GetRoom(int roomId)
        {
            var room = _context.Rooms.Find(roomId);
            if (room == null) throw new KeyNotFoundException("Room not found");
            return room;
        }

        private Room GetRoomByPublicId(string publicId)
        {
            var room = _context.Rooms.Find(publicId);
            if (room == null) throw new KeyNotFoundException("Room not found");
            return room;
        }
    }
}

