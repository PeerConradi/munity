using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MUNityClient.Services
{
    public class HttpService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;
        private string _token = null;

        public async Task<HttpClient> GetAuthClient()
        {
            if (_token == null) await CheckToken();
            return _httpClient;
        }

        public HttpClient HttpClient => this._httpClient;

        public async Task<bool> CheckToken()
        {
            var token = await GetStoredToken();
            if (string.IsNullOrEmpty(token)) return false;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("baerer", token);
            var result = await _httpClient.GetAsync($"/api/User/WhoAmI");
            var tokenValid = result.IsSuccessStatusCode;
            if (!tokenValid) await this._localStorageService.RemoveItemAsync("munity_token");
            return tokenValid;
        }

        public async Task<HttpClient> WithToken()
        {
            var token = await GetStoredToken();
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("baerer", token ?? "");
            return _httpClient;
        }

        public async Task<string> GetStoredToken()
        {
            return await this._localStorageService.GetItemAsync<string>("munity_token");
        }

        public async Task SetToken(string token)
        {
            await this._localStorageService.SetItemAsync("munity_token", token);
        }

        public async Task RemoveToken()
        {
            await this._localStorageService.RemoveItemAsync("munity_token");
        }

        public HttpService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            this._httpClient = httpClient;
            this._localStorageService = localStorageService;
            this._localStorageService.Changed += _localStorageService_Changed;
        }

        private void _localStorageService_Changed(object sender, ChangedEventArgs e)
        {
            
        }
    }
}
