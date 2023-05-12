using System;
using System.Data;
using System.Net.NetworkInformation;
using System.Text.Json.Serialization;

namespace Pokerino.Server.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }

        [JsonIgnore]
        public string? Password { get; set; }
    }
}

