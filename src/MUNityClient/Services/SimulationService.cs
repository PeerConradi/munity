﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using System.Net.Http;
using MUNity.Schema.Simulation;
using MUNitySchema.Schema.Simulation.Resolution;
using MUNityClient.ViewModels;
using MUNityClient.Extensions.Simulation;
using System.Runtime.CompilerServices;

namespace MUNityClient.Services
{
    public class SimulationService
    {
        private readonly HttpClient _httpClient;

        private readonly ILocalStorageService _localStorage;

        public async Task<bool> IsOnline()
        {
            try
            {
                var result = await this._httpClient.GetAsync($"/api/Simulation/IsOnline");
                if (result.IsSuccessStatusCode)
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<HttpResponseMessage> GetSimulationInfo(int simulationId)
        {
            return await _httpClient.GetAsync(Program.API_URL + $"/api/Simulation/Info?simulationId={simulationId}");
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
            var result = await _httpClient.PostAsync($"/api/Simulation/CreateSimulation", content);
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
            return await _httpClient.GetFromJsonAsync<List<string>>("/api/Simulation/Petition/PetitionTemplateNames");
        }

        public async Task RemoveToken(int id)
        {
            var tokens = await GetStoredTokens();
            var token = tokens.FirstOrDefault(n => n.SimulationId == id);
            if (token != null)
                tokens.Remove(token);
            await this._localStorage.SetItemAsync("munity_simsims", tokens);
        }

        public async Task<ICollection<MUNity.Schema.Simulation.SimulationListItemDto>> GetSimulationList()
        {
            return await _httpClient.GetFromJsonAsync<ICollection<MUNity.Schema.Simulation.SimulationListItemDto>>("/api/Simulation/GetListOfSimulations");
        }

        public async Task<MUNity.Schema.Simulation.SimulationTokenResponse> GetSimulationToken(int id)
        {
            var tokens = await GetStoredTokens();
            return tokens.FirstOrDefault(n => n.SimulationId == id);
        }

       

        public async Task<MUNity.Schema.Simulation.SimulationTokenResponse> JoinSimulation(MUNity.Schema.Simulation.JoinAuthenticate body)
        {
            var content = JsonContent.Create(body);
            var result = await _httpClient.PostAsync($"/api/Simulation/JoinSimulation", content);
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

        public async Task<HttpResponseMessage> MakePetition(MUNity.Schema.Simulation.CreatePetitionRequest petition)
        {
            var tokenObject = await GetSimulationToken(petition.SimulationId);
            if (tokenObject == null)
            {
                Console.WriteLine($"No token element found for MakePetition! For simulation: {petition.SimulationId}");
                return null;
            }
            petition.Token = tokenObject.Token;
            return await _httpClient.PutAsync($"/api/Simulation/Petition/MakePetition", JsonContent.Create(petition));
        }

        public async Task<HttpResponseMessage> AcceptPetition(MUNity.Schema.Simulation.CreatePetitionRequest petition)
        {
            petition.Token = (await GetSimulationToken(petition.SimulationId)).Token;
            return await _httpClient.PutAsync($"/api/Simulation/Petition/AcceptPetition", JsonContent.Create(petition));
        }

        public async Task<HttpResponseMessage> DeletePetition(int simulationId, MUNity.Schema.Simulation.PetitionDto petition)
        {
            var token = await GetSimulationToken(simulationId);
            if (token == null) return null;

            var requestBody = new MUNity.Schema.Simulation.PetitionInteractRequest()
            {
                AgendaItemId = petition.TargetAgendaItemId,
                PetitionId = petition.PetitionId,
                SimulationId = simulationId,
                Token = token.Token
            };
            return await _httpClient.PutAsync($"/api/Simulation/Petition/DeletePetition", JsonContent.Create(requestBody));
        }

        public async Task<MUNity.Schema.Simulation.SimulationDto> GetSimulation(int id, string token = null)
        {
            HttpClient client;
            if (token == null)
                client = await GetSimulationClient(id);
            else
                client = GetSimulationClient(id, token);

            if (client == null) return null;
            return await client.GetFromJsonAsync<MUNity.Schema.Simulation.SimulationDto>($"/api/Simulation/Simulation?simulationId={id}");
        }

        public async Task<HttpResponseMessage> CloseAllConnections(int simulationId)
        {
            var token = await this.GetSimulationToken(simulationId);
            if (token == null) return null;
            var body = new SimulationRequest()
            {
                SimulationId = simulationId,
                Token = token.Token
            };
            return await this._httpClient.PutAsJsonAsync($"/api/Simulation/CloseAllConnections", body);
        }

        public async Task<HttpResponseMessage> CreateAgendaItem(CreateAgendaItemDto dto)
        {
            var token = await GetSimulationToken(dto.SimulationId);
            if (token == null) return null;
            dto.Token = token.Token;
            return await _httpClient.PostAsJsonAsync("/api/Simulation/AgendaItem/CreateAgendaItem", dto);
        }

        public async Task SecureAgendaItems(SimulationViewModel viewModel)
        {
            var client = GetSimulationClient(viewModel);
            var response = await client.GetAsync($"/api/Simulation/AgendaItem/AgendaItems?simulationId={viewModel.Simulation.SimulationId}&withPetitions=true");
            if (response.IsSuccessStatusCode)
            {
                viewModel.AgendaItems = await response.Content.ReadFromJsonAsync<List<AgendaItemDto>>();
            }
            else
            {
                NotifyError(viewModel, response);
            }
        }

        public async Task StoreOpenedTab(int simulationId, int tabId)
        {
            string name = "simlastopenedtab";
            var tabs = await _localStorage.GetItemAsync<List<MUNityClient.Models.Simulation.LastOpenedTab>>(name);
            if (tabs == null)
                tabs = new List<Models.Simulation.LastOpenedTab>();
            var content = tabs.FirstOrDefault(n => n.SimulationId == simulationId);
            if (content == null)
            {
                content = new Models.Simulation.LastOpenedTab() { SimulationId = simulationId };
                tabs.Add(content);
            }             
            content.TabId = tabId;
            await _localStorage.SetItemAsync<List<MUNityClient.Models.Simulation.LastOpenedTab>>(name, tabs);
        }

        public async Task<int> GetLastOpenedTab(int simulationId)
        {
            string name = "simlastopenedtab";
            var tabs = await _localStorage.GetItemAsync<List<MUNityClient.Models.Simulation.LastOpenedTab>>(name);
            if (tabs == null)
                return 0;
            var content = tabs.FirstOrDefault(n => n.SimulationId == simulationId);
            if (content == null) return 0;
            return content.TabId;
        }

        public async Task<HttpResponseMessage> ApplyPetitionTemplate(int simulationId, string name)
        {
            var template = new ApplyPetitionTemplate()
            {
                Name = name,
                SimulationId = simulationId,
                Token = (await GetSimulationToken(simulationId)).Token
            };
            return await this._httpClient.PutAsJsonAsync<ApplyPetitionTemplate>("/api/Simulation/Petition/ApplyPetitionPreset", template);
        }

        public async Task SecurePetitionTypes(SimulationViewModel viewModel)
        {
            var client = GetSimulationClient(viewModel);
            var response = await client.GetAsync($"/api/Simulation/Petition/SimulationPetitionTypes?simulationId={viewModel.Simulation.SimulationId}");
            if (response.IsSuccessStatusCode)
            {
                viewModel.PetitionTypes = await response.Content.ReadFromJsonAsync<List<PetitionTypeSimulationDto>>();
            }
            else
            {
                NotifyError(viewModel, response);
            }
        }

        private void NotifyError(SimulationViewModel viewModel, HttpResponseMessage response, [CallerMemberName] string caller = "SimulationService")
        {
            viewModel.ShowError("Error", $"Error in {caller} ({response.StatusCode} - {response.ReasonPhrase})");
        }

        public async Task<List<MUNity.Schema.Simulation.SimulationUserAdminDto>> GetUserSetups(int simulationId)
        {
            var client = await GetSimulationClient(simulationId);
            if (client == null) return null;
            var subs = await client.GetFromJsonAsync<List<MUNity.Schema.Simulation.SimulationUserAdminDto>>($"/api/Simulation/GetUsersAsAdmin?id={simulationId}");
            return subs;
        }

        public async Task<ICollection<MUNity.Schema.Simulation.SimulationUserDefaultDto>> GetUsers(int simulationId)
        {
            var client = await GetSimulationClient(simulationId);
            if (client == null) return null;
            return await client.GetFromJsonAsync<ICollection<MUNity.Schema.Simulation.SimulationUserDefaultDto>>($"/api/Simulation/GetUsersDefault?id={simulationId}");
        }

        //public async Task SecureGetMyAuth(SimulationViewModel viewModel)
        //{
        //    var client = GetSimulationClient(viewModel);
        //    var response = await client.GetAsync($"/api/Simulation/GetSimulationAuth?id={viewModel.Simulation.SimulationId}");
        //    if (response.IsSuccessStatusCode)
        //    {
        //        viewModel.MyAuth = await response.Content.ReadFromJsonAsync<SimulationAuthDto>();
        //    }
        //    else
        //    {
        //        viewModel.ShowError("Unable to get Authentication", $"Problem when loading your authentication for the simulation. Error code: {response.StatusCode}");
        //    }
        //}

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

        public async Task<bool> CanCreateSimulation()
        {
            var client = new HttpClient();
            var result = await client.GetAsync(Program.API_URL + "/api/Simulation/CanCreateSimulation");
            if (!result.IsSuccessStatusCode) return false;
            var canCreate = await result.Content.ReadAsStringAsync();
            return canCreate == "true";
        }

        public async Task<List<SimulationRolesPreset>> GetPresets()
        {
            return await _httpClient.GetFromJsonAsync<List<SimulationRolesPreset>>("/api/Simulation/GetPresets");
        }

        public async Task<List<ResolutionSmallInfo>> GetSimulationResolutionInfos(int simulationId, string token = null)
        {
            HttpClient client;
            if (token == null)
                client = await GetSimulationClient(simulationId);
            else
                client = GetSimulationClient(simulationId, token);
            return await client.GetFromJsonAsync<List<ResolutionSmallInfo>>($"/api/Simulation/SimulationResolutions?simulationId={simulationId}");
        }

        public async Task SecureGetRoles(SimulationViewModel viewModel)
        {
            var client = GetSimulationClient(viewModel.Simulation.SimulationId, viewModel.Token);
            var response = await client.GetAsync($"/api/Simulation/Roles/GetSimulationRoles?id={viewModel.Simulation.SimulationId}");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    viewModel.Simulation.Roles = await response.Content.ReadFromJsonAsync<List<MUNity.Schema.Simulation.SimulationRoleDto>>();
                    viewModel.NotifyRolesChanged();
                }
                catch (Exception ex)
                {
                    viewModel.ShowError("Unable to load roles", $"Error when parsing the incoming result. {nameof(SimulationService)} {nameof(SecureGetRoles)}");
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public async Task ApplyPreset(int simulationId, string presetId)
        {
            var client = await GetSimulationClient(simulationId);
            if (client == null) throw new Exception();
            await client.GetAsync($"/api/Simulation/ApplyPreset?simulationId={simulationId}&presetId={presetId}");
        }



        public async Task<MUNity.Schema.Simulation.SimulationUserAdminDto> CreateUser(int simulationId)
        {
            var client = await GetSimulationClient(simulationId);
            if (client == null) throw new Exception();
            return await client.GetFromJsonAsync<MUNity.Schema.Simulation.SimulationUserAdminDto>($"/api/Simulation/CreateUser?id={simulationId}");
        }

        public async Task<HttpResponseMessage> RemoveUser(int simulationId, int userId)
        {
            var client = await GetSimulationClient(simulationId);
            if (client == null) throw new Exception();
            return await client.GetAsync($"/api/Simulation/RemoveSimulationUser?simulationId={simulationId}&userId={userId}");
        }

        public async Task<HttpResponseMessage> CreateVote(MUNity.Schema.Simulation.CreateSimulationVoting model)
        {
            model.Token = (await GetSimulationToken(model.SimulationId))?.Token ?? "";
            Console.WriteLine(model.Token);
            Console.WriteLine(model);
            Console.WriteLine(model.SimulationId);
            return await _httpClient.PostAsJsonAsync<MUNity.Schema.Simulation.CreateSimulationVoting>($"/api/Simulation/CreateVoting", model);
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
            var client = this._httpClient;
            
            if (client.DefaultRequestHeaders.Contains("simsimtoken"))
            {
                client.DefaultRequestHeaders.Remove("simsimtoken");
            }

            client.DefaultRequestHeaders.Add("simsimtoken", token.Token);
            return client;
        }

        private HttpClient GetSimulationClient(SimulationViewModel viewModel)
        {
            var client = this._httpClient;
            if (client.DefaultRequestHeaders.Contains("simsimtoken"))
            {
                client.DefaultRequestHeaders.Remove("simsimtoken");
            }

            client.DefaultRequestHeaders.Add("simsimtoken", viewModel.Token);
            return client;
        }

        private HttpClient GetSimulationClient(int simulationId, string token)
        {
            var client = this._httpClient;
            if (client.DefaultRequestHeaders.Contains("simsimtoken"))
            {

                // Its easier to just remove this header element and create a newone than
                // it is to search of there is more than one value behind it.
                client.DefaultRequestHeaders.Remove("simsimtoken");
            }

            client.DefaultRequestHeaders.Add("simsimtoken", token);
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
        public async Task<MUNityClient.ViewModels.SimulationViewModel> Subscribe(int simulationId, string masterToken = null)
        {
            string accessToken = masterToken;
            if (accessToken == null)
            {
                var token = await GetSimulationToken(simulationId);
                accessToken = token.Token;
                if (token == null) return null;
            }

            var simulation = await this.GetSimulation(simulationId, accessToken);
            if (simulation == null) return null;

            var viewModel = await MUNityClient.ViewModels.SimulationViewModel.CreateViewModel(simulation, this, accessToken);
            viewModel.Token = accessToken;
            var connId = viewModel.HubConnection.ConnectionId;
            var subscribeBody = new MUNity.Schema.Simulation.SubscribeSimulation()
            {
                SimulationId = simulation.SimulationId,
                ConnectionId = connId,
                Token = accessToken
            };

            var result = await _httpClient.PostAsync($"/api/Simulation/Subscribe", JsonContent.Create(subscribeBody));
            if (result.IsSuccessStatusCode)
                return viewModel;
            return null;
        }

        public async Task<bool> IsUserOnline(int simulationId, int userId)
        {
            var client = await GetSimulationClient(simulationId);
            if (client == null) throw new Exception();
            return await client.GetFromJsonAsync<bool>($"/api/Simulation/IsUserOnline?simulationId={simulationId}&userId={userId}");
        }

        public SimulationService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            this._httpClient = httpClient;
            this._localStorage = localStorage;
        }
    }
}
