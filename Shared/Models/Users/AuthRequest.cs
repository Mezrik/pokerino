using System.ComponentModel.DataAnnotations;

namespace Pokerino.Shared.Models.Users
{
    public class AuthRequest
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}

