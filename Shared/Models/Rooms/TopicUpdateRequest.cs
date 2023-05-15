using System;
using System.ComponentModel.DataAnnotations;

namespace Pokerino.Shared.Models.Rooms
{
    public class TopicUpdateRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Id { get; set; }
    }
}

