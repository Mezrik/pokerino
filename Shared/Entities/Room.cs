namespace Pokerino.Shared.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string PublicId { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        public RoomUser Host { get; set; }
        public ICollection<RoomUser> Users { get; set; } = new List<RoomUser>();
        public RoomStatus Status { get; set; } = RoomStatus.NotStarted;
        public ICollection<RoomTopic> Topics { get; set; } = new List<RoomTopic>();
    }
}
