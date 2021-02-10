using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using System.Net.Http;
using MUNity.Schema.Simulation;
using MUNitySchema.Schema.Simulation.Resolution;

namespace MUNityClient.Services
{
    public class SimulationService
    {
        private readonly HttpService _httpService;

        private readonly ILocalStorageService _localStorage;

        public async Task<bool> IsOnline()
        {
            try
            {
                var result = await this._httpService.HttpClient.GetAsync($"/api/Simulation/IsOnline");
                if (result.IsSuccessStatusCode)
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Creates a Simulation and returns its token. The token will also be stored inside the 
        /// munity_simsims local storage.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public async Task<MUNity.Schema.Simulation.CreateSimulationResponse> CreateSimulation(MUNity.Schema.Simulation.CreateSimulationRequest schema)
        {
            var content = JsonContent.Create(schema);
            var result = await this._httpService.HttpClient.PostAsync($"/api/Simulation/CreateSimulation", content);
            if (result.IsSuccessStatusCode)
            {
                var sim = await result.Content.ReadFromJsonAsync<MUNity.Schema.Simulation.CreateSimulationResponse>();
                var token2 = new MUNity.Schema.Simulation.SimulationTokenResponse()
                {
                    Name = sim.SimulationName,
                    SimulationId = sim.SimulationId,
                    Token = sim.FirstUserToken
                };
                await StoreToken(token2);
                return sim;
            }
            throw new Exception("something went wrong");
        }

        

        public async Task<ICollection<MUNity.Schema.Simulation.SimulationTokenResponse>> GetStoredTokens()
        {
            return await this._localStorage.GetItemAsync<ICollection<MUNity.Schema.Simulation.SimulationTokenResponse>>("munity_simsims");
        }

        public async Task<List<string>> GetPetitionPresetNames()
        {
            return await this._httpService.HttpClient.GetFromJsonAsync<List<string>>("/api/Simulation/PetitionTemplateNames");
        }

        public async Task RemoveToken(int id)
        {
            var tokens = await GetStoredTokens();
            var token = tokens.FirstOrDefault(n => n.SimulationId == id);
            if (token != null)
                tokens.Remove(token);
            await this._localStorage.SetItemAsync("munity_simsims", tokens);
        }

        public async Task<ICollection<MUNity.Schema.Simulation.SimulationListItem>> GetSimulationList()
        {
            return await this._httpService.HttpClient.GetFromJsonAsync<ICollection<MUNity.Schema.Simulation.SimulationListItem>>("/api/Simulation/GetListOfSimulations");
        }

        public async Task<MUNity.Schema.Simulation.SimulationTokenResponse> GetSimulationToken(int id)
        {
            var tokens = await GetStoredTokens();
            return tokens.FirstOrDefault(n => n.SimulationId == id);
        }

       

        public async Task<MUNity.Schema.Simulation.SimulationTokenResponse> JoinSimulation(MUNity.Schema.Simulation.JoinAuthenticate body)
        {
            var content = JsonContent.Create(body);
            var result = await this._httpService.HttpClient.PostAsync($"/api/Simulation/JoinSimulation", content);
            if (result.IsSuccessStatusCode)
            {
                var token = await result.Content.ReadFromJsonAsync<MUNity.Schema.Simulation.SimulationTokenResponse>();
                await StoreToken(token);
                return token;
            }
            throw new Exception("something went wrong");
        }

        public async Task PickRole(int simulationId, int roleId)
        {
            var client = await GetSimulationClient(simulationId);
            if (client == null) throw new Exception();
            await client.GetAsync($"/api/Simulation/PickRole?simulationId={simulationId}&roleId={roleId}");
        }

        public async Task<HttpResponseMessage> SetPhase(int simulationId, GamePhases phase)
        {
            var client = await GetSimulationClient(simulationId);
            if (client == null) throw new Exception();
            var body = new SetPhaseRequest();
            body.Token = (await GetSimulationToken(simulationId)).Token;
            body.SimulationId = simulationId;
            body.SimulationPhase = phase;
            return await client.PutAsJsonAsync<SetPhaseRequest>($"/api/Simulation/SetPhase", body);
        }

        public async Task<HttpResponseMessage> SetPhase(int simulationId, MUNity.Schema.Simulation.SetPhaseRequest phase)
        {
            var client = await GetSimulationClient(simulationId);
            if (client == null) throw new Exception();
            return await client.GetAsync($"/api/Simulation/SetPhase?simulationId={simulationId}&phase={phase}");
        }

        public async Task<HttpResponseMessage> MakePetition(MUNity.Schema.Simulation.PetitionDto petition)
        {
            petition.Token = (await GetSimulationToken(petition.SimulationId)).Token;
            return await _httpService.HttpClient.PutAsync($"/api/Simulation/MakePetition", JsonContent.Create(petition));
        }

        public async Task<HttpResponseMessage> AcceptPetition(MUNity.Schema.Simulation.PetitionDto petition)
        {
            petition.Token = (await GetSimulationToken(petition.SimulationId)).Token;
            return await _httpService.HttpClient.PutAsync($"/api/Simulation/AcceptPetition", JsonContent.Create(petition));
        }

        public async Task<HttpResponseMessage> DeletePetition(MUNity.Schema.Simulation.PetitionDto petition)
        {
            petition.Token = (await GetSimulationToken(petition.SimulationId)).Token;
            return await _httpService.HttpClient.PutAsync($"/api/Simulation/DeletePetition", JsonContent.Create(petition));
        }

        public async Task<MUNity.Schema.Simulation.SimulationResponse> GetSimulation(int id)
        {
            var client = await GetSimulationClient(id);
            if (client == null) return null;
            return await client.GetFromJsonAsync<MUNity.Schema.Simulation.SimulationResponse>($"/api/Simulation/GetSimulation?id={id}");
        }

        public async Task<HttpResponseMessage> ApplyPetitionTemplate(int simulationId, string name)
        {
            var client = this._httpService.HttpClient;
            var template = new ApplyPetitionTemplate()
            {
                Name = name,
                SimulationId = simulationId,
                Token = (await GetSimulationToken(simulationId)).Token
            };
            return await client.PutAsJsonAsync<ApplyPetitionTemplate>("/api/Simulation/ApplyPetitionPreset", template);
        }

        public async Task<List<PetitionTypeSimulationDto>> PetitionTypes(int simulationId)
        {
            var client = await GetSimulationClient(simulationId);
            return await client.GetFromJsonAsync<List<PetitionTypeSimulationDto>>($"/api/Simulation/SimulationPetitionTypes?simulationId={simulationId}");
        }

        public async Task<List<MUNity.Schema.Simulation.SimulationUserSetup>> GetUserSetups(int simulationId)
        {
            var client = await GetSimulationClient(simulationId);
            if (client == null) return null;
            var subs = await client.GetFromJsonAsync<List<MUNity.Schema.Simulation.SimulationUserSetup>>($"/api/Simulation/GetUsersAsAdmin?id={simulationId}");
            return subs;
        }

        public async Task<ICollection<MUNity.Schema.Simulation.SimulationUserItem>> GetUsers(int simulationId)
        {
            var client = await GetSimulationClient(simulationId);
            if (client == null) return null;
            return await client.GetFromJsonAsync<ICollection<MUNity.Schema.Simulation.SimulationUserItem>>($"/api/Simulation/GetUsersDefault?id={simulationId}");
        }

        public async Task<MUNity.Schema.Simulation.SimulationAuthSchema> GetMyAuth(int id)
        {
            var client = await GetSimulationClient(id);
            if (client == null) return null;
            return await client.GetFromJsonAsync<MUNity.Schema.Simulation.SimulationAuthSchema>($"/api/Simulation/GetSimulationAuth?id={id}");
        }

        public async Task SetUserRole(int simulationId, int userId, int roleId)
        {
            var client = await GetSimulationClient(simulationId);
            if (client == null) return;
            await client.GetAsync($"/api/Simulation/SetUserRole?simulationId={simulationId}&userId={userId}&roleId={roleId}");
        }

        public async Task<string> GetListOfSpeakerId(int simulationId)
        {
            var client = await GetSimulationClient(simulationId);
            if (client == null) return null;
            return await client.GetStringAsync($"/api/Simulation/GetListOfSpeakersId?simulationId={simulationId}");
        }

        public async Task<string> GetResolutionId(int simulationId)
        {
            var client = await GetSimulationClient(simulationId);
            if (client == null) return null;
            return await client.GetStringAsync($"/api/Simulation/GetResolutionId?simulationId={simulationId}");
        }

        public async Task<MUNity.Models.ListOfSpeakers.ListOfSpeakers> InitListOfSpeakers(int simulationId)
        {
            var client = await GetSimulationClient(simulationId);
            if (client == null) return null;
            return await client.GetFromJsonAsync<MUNity.Models.ListOfSpeakers.ListOfSpeakers>($"/api/Simulation/InitListOfSpeakers?simulationId={simulationId}");
        }

        public async Task<List<SimulationRolesPreset>> GetPresets()
        {
            var client = this._httpService.HttpClient;
            return await client.GetFromJsonAsync<List<SimulationRolesPreset>>("/api/Simulation/GetPresets");
        }

        public async Task<List<ResolutionSmallInfo>> GetSimulationResolutionInfos(int simulationId)
        {
            var client = await GetSimulationClient(simulationId);
            if (client == null) return null;
            return await client.GetFromJsonAsync<List<ResolutionSmallInfo>>($"/api/Simulation/SimulationResolutions?simulationId={simulationId}");
        }

        public async Task<List<MUNity.Schema.Simulation.SimulationRoleItem>> GetRoles(int id)
        {
            var client = await GetSimulationClient(id);
            if (client == null) return null;
            return await client.GetFromJsonAsync<List<MUNity.Schema.Simulation.SimulationRoleItem>>($"/api/Simulation/GetSimulationRoles?id={id}");
        }

        public async Task ApplyPreset(int simulationId, string presetId)
        {
            var client = await GetSimulationClient(simulationId);
            if (client == null) throw new Exception();
            await client.GetAsync($"/api/Simulation/ApplyPreset?simulationId={simulationId}&presetId={presetId}");
        }



        public async Task<MUNity.Schema.Simulation.SimulationUserSetup> CreateUser(int simulationId)
        {
            var client = await GetSimulationClient(simulationId);
            if (client == null) throw new Exception();
            return await client.GetFromJsonAsync<MUNity.Schema.Simulation.SimulationUserSetup>($"/api/Simulation/CreateUser?id={simulationId}");
        }

        public async Task<HttpResponseMessage> RemoveUser(int simulationId, int userId)
        {
            var client = await GetSimulationClient(simulationId);
            if (client == null) throw new Exception();
            return await client.GetAsync($"/api/Simulation/RemoveSimulationUser?simulationId={simulationId}&userId={userId}");
        }

        public async Task<HttpResponseMessage> CreateVote(MUNity.Schema.Simulation.CreateSimulationVoting model)
        {
            var client = _httpService.HttpClient;
            model.Token = (await GetSimulationToken(model.SimulationId))?.Token ?? "";
            Console.WriteLine(model.Token);
            Console.WriteLine(model);
            Console.WriteLine(model.SimulationId);
            return await client.PostAsJsonAsync<MUNity.Schema.Simulation.CreateSimulationVoting>($"/api/Simulation/CreateVoting", model);
        }

        public async Task<HttpResponseMessage> Vote(int simulationId, string voteId, int choice)
        {
            var client = await GetSimulationClient(simulationId);
            if (client == null) return null;
            return await client.GetAsync($"/api/Simulation/Vote?simulationId={simulationId}&voteId={voteId}&choice={choice}");
        }

        private async Task<HttpClient> GetSimulationClient(int id)
        {
            var token = await GetSimulationToken(id);
            if (token == null) return null;
            var client = this._httpService.HttpClient;
            if (client.DefaultRequestHeaders.Contains("simsimtoken"))
            {
                // Its easier to just remove this header element and create a newone than
                // it is to search of there is more than one value behind it.
                client.DefaultRequestHeaders.Remove("simsimtoken");
            }

            client.DefaultRequestHeaders.Add("simsimtoken", token.Token);
            return client;
        }



        public async Task StoreToken(MUNity.Schema.Simulation.SimulationTokenResponse token)
        {
            if (token == null)
                throw new ArgumentException("You have to pass a token here!");
            var tokens = await this._localStorage.GetItemAsync<ICollection<MUNity.Schema.Simulation.SimulationTokenResponse>>("munity_simsims");
            if (tokens == null)
                tokens = new List<MUNity.Schema.Simulation.SimulationTokenResponse>();

            MUNity.Schema.Simulation.SimulationTokenResponse element = null;
            if (tokens.Any())
                element = tokens.FirstOrDefault(n => n.SimulationId == token.SimulationId);
            if (element != null)
            {
                element.Name = token.Name;
                element.Token = token.Token;
            }
            else
            {
                tokens.Add(token);
            }
            await this._localStorage.SetItemAsync("munity_simsims", tokens);
        }

        /// <summary>
        /// Creates a kind of Simulation ViewModel that holds the Simulation itself and all the relevant information
        /// </summary>
        /// <param name="simulationId"></param>
        /// <returns></returns>
        public async Task<MUNityClient.ViewModel.SimulationViewModel> Subscribe(int simulationId)
        {
            var token = await GetSimulationToken(simulationId);
            if (token == null) return null;

            var simulation = await this.GetSimulation(simulationId);
            if (simulation == null) return null;
            simulation.Roles = await this.GetRoles(simulationId);

            

            var auth = await this.GetMyAuth(simulationId);

            var defaultUsers = await this.GetUsers(simulationId);

            var currentUser = defaultUsers.FirstOrDefault(n => n.SimulationUserId == auth.SimulationUserId);
            var myRole = simulation.Roles.FirstOrDefault(n => n.SimulationRoleId == auth.SimulationUserId);

            // Load users extra depending on auth
            if (auth.CanCreateRole || (myRole != null && myRole.RoleType == RoleTypes.Chairman))
            {
                var tmpUsers = await this.GetUserSetups(simulationId);
                simulation.Users.Clear();
                simulation.Users.AddRange(tmpUsers);
                var meInThisList = tmpUsers.FirstOrDefault(n => n.SimulationUserId == auth.SimulationUserId);
                if (meInThisList != null) meInThisList.IsOnline = true;
            }
            else
            {
                simulation.Users.Clear();
                simulation.Users.AddRange(defaultUsers);
                var meInThisList = defaultUsers.FirstOrDefault(n => n.SimulationUserId == auth.SimulationUserId);
                if (meInThisList != null) meInThisList.IsOnline = true;
            }

            var socket = await MUNityClient.ViewModel.SimulationViewModel.CreateHander(simulation, auth, this);
            var connId = socket.HubConnection.ConnectionId;
            var subscribeBody = new MUNity.Schema.Simulation.SubscribeSimulation()
            {
                SimulationId = simulation.SimulationId,
                ConnectionId = connId,
                Token = token.Token
            };
            var client = _httpService.HttpClient;
            var result = await client.PostAsync($"/api/Simulation/Subscribe", JsonContent.Create(subscribeBody));
            if (result.IsSuccessStatusCode)
                return socket;
            return null;
        }

        public async Task<bool> IsUserOnline(int simulationId, int userId)
        {
            var client = await GetSimulationClient(simulationId);
            if (client == null) throw new Exception();
            return await client.GetFromJsonAsync<bool>($"/api/Simulation/IsUserOnline?simulationId={simulationId}&userId={userId}");
        }

        public SimulationService(HttpService httpService, ILocalStorageService localStorage)
        {
            this._httpService = httpService;
            this._localStorage = localStorage;
        }
    }
}
