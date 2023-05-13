using System;
using Pokerino.Shared.Entities;
using System.ComponentModel.Design;
using static System.Net.WebRequestMethods;
using System.Net.Http.Json;
using Pokerino.Shared.Models.Users;

namespace Pokerino.Client.Services
{
    public interface IUserService
    {
        Task<User[]?> GetAll();
        Task<HttpResponseMessage> CreateUser(UserCreateRequest model);
    }

    public class UserService : IUserService
    {
        private HttpClient _http;

        public UserService(HttpClient http)
        {
            _http = http;
        }

        public async Task<User[]?> GetAll()
        {
            return await _http.GetFromJsonAsync<User[]>("users/all");
        }

        public async Task<HttpResponseMessage> CreateUser(UserCreateRequest model)
        {
            return await _http.PostAsJsonAsync<UserCreateRequest>("users/create", model);
        }
    }
}

