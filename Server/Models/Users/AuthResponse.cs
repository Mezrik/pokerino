using System;
using Pokerino.Shared.Entities;

namespace Pokerino.Server.Models.Users
{
    public class AuthResponse
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string Token { get; set; }


        public AuthResponse(User user, string token)
        {
            Id = user.Id;
            Email = user.Email;
            Username = user.Username;
            Token = token;
        }
    }
}

