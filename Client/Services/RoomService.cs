using System;
using Pokerino.Shared.Models.Users;
using System.Net.Http.Json;
using Pokerino.Shared.Models.Rooms;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Pokerino.Client.Services
{
    public interface IRoomService
    {
        Task<bool> CheckIfExists(string publicId);
        Task<HttpResponseMessage> CreateRoom(RoomCreateRequest model);
        Task<List<Pokerino.Shared.Entities.Room>> GetAllUserRooms(int userId);
    }

    public class RoomService : IRoomService
    {
        private HttpClient _http;
        private IAuthenticationService _authService;

        public RoomService(HttpClient http, IAuthenticationService authService)
        {
            _http = http;
            _authService = authService;
        }

        public async Task<HttpResponseMessage> CreateRoom(RoomCreateRequest model)
        {
            if (_authService.User is null) model.Username = _authService.AnonymousUser?.Username;

            return await _http.PostAsJsonAsync<RoomCreateRequest>($"rooms/create", model);
        }

        public async Task<bool> CheckIfExists(string publicId)
        {
            return await _http.GetFromJsonAsync<bool>($"rooms/{publicId}/exists");
        }

        public async Task<List<Pokerino.Shared.Entities.Room>> GetAllUserRooms(int userId)
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _authService.User?.Token);
            return await _http.GetFromJsonAsync<List<Pokerino.Shared.Entities.Room>>($"rooms/user/{userId}") ?? new();
        }
    }
}

