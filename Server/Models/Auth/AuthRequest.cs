﻿using System.ComponentModel.DataAnnotations;

namespace Pokerino.Server.Models
{
    public class AuthRequest
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}

