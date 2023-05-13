using System;
namespace Pokerino.Shared.Entities
{
    public class RoomUser
    {
        public int Id { get; set; }
        public User? User { get; set; }
        public string? Name { get; set; }
        public Role Role { get; set; }
    }
}

