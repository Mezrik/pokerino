using System;
using Microsoft.AspNetCore.Components;
using Pokerino.Shared.Entities;
using System.ComponentModel.Design;
using System.Net.Http.Json;
using Pokerino.Shared.Models.Users;

namespace Pokerino.Client.Services
{
    public interface IAuthenticationService
    {
        AuthResponse? User { get; }
        Task Initialize();
        Task Login(string username, string password);
        Task Logout();
    }

    public class AuthenticationService : IAuthenticationService
    {
        private HttpClient _http;
        private NavigationManager _navigationManager;
        private ICookieService _cookieService;

        public AuthResponse? User { get; private set; }

        public AuthenticationService(
            NavigationManager navigationManager,
            ICookieService cookieService,
            HttpClient http
        )
        {
            _http = http;
            _navigationManager = navigationManager;
            _cookieService = cookieService;
        }

        public async Task Initialize()
        {
            User = await _cookieService.GetValueAsync<AuthResponse>("user");
        }

        public async Task Login(string username, string password)
        {
            var response = await _http.PostAsJsonAsync(
                "/users/authenticate",
                new { username, password }
            );

            User = await response.Content.ReadFromJsonAsync<AuthResponse>();

            await _cookieService.SetValueAsync("user", User);
        }

        public async Task Logout()
        {
            User = null;
            await _cookieService.RemoveValueAsync("user");
            _navigationManager.NavigateTo("login");
        }
    }
}

