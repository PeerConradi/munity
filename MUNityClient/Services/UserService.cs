using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;

namespace MUNityClient.Services
{

    /// <summary>
    /// The user Service is handling the requests sent to  the API Controller: User.
    /// This Service allows to Login or register.
    /// </summary>
    public class UserService
    {
        public delegate void OnLoggedIn(MUNity.Schema.Authentication.AuthenticationResponse user);

        public event OnLoggedIn UserLoggedIn;

        private readonly HttpService _httpService;

        private MUNity.Schema.User.UserInformation loggedInUser;

        public MUNity.Schema.User.UserInformation CurrentUser => loggedInUser;

        public async Task<MUNity.Schema.User.UserInformation> GetMyself()
        {
            var token = await _httpService.GetStoredToken();
            if (string.IsNullOrEmpty(token)) return null;
            var httpClient = await this._httpService.WithToken();
            var user = await httpClient.GetFromJsonAsync<MUNity.Schema.User.UserInformation>($"/api/User/WhoAmI");
            return user;
        }

        public async Task<MUNity.Schema.Authentication.AuthenticationResponse> Login(MUNity.Schema.Authentication.AuthenticateRequest request)
        {
            var content = JsonContent.Create(request);
            var response = await this._httpService.HttpClient.PostAsync($"/api/User/Login", content);
            if (!response.IsSuccessStatusCode) return null;

            var user = await response.Content.ReadFromJsonAsync<MUNity.Schema.Authentication.AuthenticationResponse>();
            if (user != null)
            {
                await this._httpService.SetToken(user.Token);
                this.UserLoggedIn?.Invoke(user);
                return user;
            }
            return null;
        }

        public async Task<HttpResponseMessage> Register(MUNity.Schema.Authentication.RegisterRequest request)
        {
            var body = JsonContent.Create(request);
            return await this._httpService.HttpClient.PostAsync($"/api/User/Register", body);
        }

        public async Task Logout()
        {
            await this._httpService.RemoveToken();
            this.loggedInUser = null;
        }

        public UserService(HttpService httpService)
        {
            this._httpService = httpService;
        }
    }
}
