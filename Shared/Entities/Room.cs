namespace Pokerino.Shared.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string PublicId { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";

        public ICollection<RoomUser> RoomUsers { get; set; }

        public RoomStatus Status { get; set; } = RoomStatus.NotStarted;

        public RoomTopic? ActiveTopic { get; set; }
        public ICollection<RoomTopic> Topics { get; set; } = new List<RoomTopic>();
    }
}
