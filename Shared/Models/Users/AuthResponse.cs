using System;
using System.Text.Json.Serialization;
using Pokerino.Shared.Entities;

namespace Pokerino.Shared.Models.Users
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

        [JsonConstructorAttribute]
        public AuthResponse(int id, string email, string username, string token)
        {
            Id = id;
            Email = email;
            Username = username;
            Token = token;
        }
    }
}

