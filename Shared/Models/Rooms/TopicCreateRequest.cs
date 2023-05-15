using System;
using System.ComponentModel.DataAnnotations;

namespace Pokerino.Shared.Models.Rooms
{
    public class TopicCreateRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}

