using System;
using Pokerino.Shared.Entities;
using System.ComponentModel.DataAnnotations;

namespace Pokerino.Server.Models.Users
{
    public class UserCreateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}

