using System;
namespace Pokerino.Shared.Entities
{
    public class RoomTopic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool ShowVotes { get; set; }

        public double? Estimate { get; set; }
        public ICollection<EstimateVote> Votes { get; set; }
    }
}

