using System;
using System.Text.Json;
using Microsoft.JSInterop;

namespace Pokerino.Client.Services
{
    public interface ICookieService
    {
        public Task<T?> GetValueAsync<T>(string key);
        public Task SetValueAsync<T>(string key, T value);
        public Task RemoveValueAsync(string key);
    }

    public class CookieService : ICookieService
    {
        private Lazy<IJSObjectReference> _accessorJsRef = new();
        private readonly IJSRuntime _jsRuntime;

        public CookieService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        private async Task WaitForReference()
        {
            if (_accessorJsRef.IsValueCreated is false)
            {
                _accessorJsRef = new(await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/CookieAccessor.js"));
            }
        }

        public async Task<T?> GetValueAsync<T>(string key)
        {
            await WaitForReference();
            var json = await _accessorJsRef.Value.InvokeAsync<string>("get", key);

            if (json == null)
                return default;

            return JsonSerializer.Deserialize<T>(json);
        }

        public async Task SetValueAsync<T>(string key, T value)
        {
            await WaitForReference();
            await _accessorJsRef.Value.InvokeVoidAsync("set", key, JsonSerializer.Serialize(value));
        }

        public async Task RemoveValueAsync(string key)
        {
            await WaitForReference();
            await _accessorJsRef.Value.InvokeVoidAsync("remove", key);
        }
    }
}

