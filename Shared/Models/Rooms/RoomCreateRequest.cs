using System;
using System.ComponentModel.DataAnnotations;

namespace Pokerino.Shared.Models.Rooms
{
    public class RoomCreateRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string PublicId { get; set; }

        public string? Username { get; set; }
    }
}

