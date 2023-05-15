using System;
using Microsoft.AspNetCore.Components;
using Pokerino.Shared.Entities;
using System.ComponentModel.Design;
using System.Net.Http.Json;
using Pokerino.Shared.Models.Users;
using Blazorise;

namespace Pokerino.Client.Services
{
    public interface IAuthenticationService
    {
        AuthResponse? User { get; }
        AnonymousUser? AnonymousUser { get; }

        Task Initialize();
        Task<HttpResponseMessage> Login(string username, string password);
        Task AnonymousLogin(string username);
        Task Logout();
    }

    public class AuthenticationService : IAuthenticationService
    {
        private HttpClient _http;
        private NavigationManager _navigationManager;
        private ICookieService _cookieService;

        public AuthResponse? User { get; private set; }
        public AnonymousUser? AnonymousUser { get; private set; }

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
            AnonymousUser = await _cookieService.GetValueAsync<AnonymousUser>("anonymousUser");
        }

        public async Task<HttpResponseMessage> Login(string username, string password)
        {
            var response = await _http.PostAsJsonAsync(
                "/users/authenticate",
                new { username, password }
            );

            User = await response.Content.ReadFromJsonAsync<AuthResponse>();

            await _cookieService.SetValueAsync("user", User);

            return response;
        }

        public async Task AnonymousLogin(string username)
        {
            AnonymousUser = new(username);
            await _cookieService.SetValueAsync("anonymousUser", AnonymousUser);
        }

        public async Task Logout()
        {
            User = null;
            await _cookieService.RemoveValueAsync("user");
            _navigationManager.NavigateTo("login");
        }
    }

    public class AnonymousUser
    {
        public string Username { get; set; }
        public AnonymousUser(string username)
        {
            Username = username;
        }
    }
}

