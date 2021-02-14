using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using MUNity.Models.Resolution;
using Blazored.LocalStorage;
using Microsoft.JSInterop;
using MUNityClient.Models.Resolution;
using MUNity.Extensions.ResolutionExtensions;
using MUNity.Models.Resolution.EventArguments;

namespace MUNityClient.Services
{
    public class ResolutionService : IResolutionService
    {
        private readonly ILocalStorageService _localStorage;

        private readonly HttpService _httpService;

        private bool? _isOnline = null;

        private DateTime? _lastOnlineChecked;

        public delegate void OnStorageChanged();

        public event OnStorageChanged StorageChanged;

        #region "Header"

        public Task<HttpResponseMessage> UpdateResolutionHeaderName(HeaderStringPropChangedEventArgs args)
        {
            return this._httpService.HttpClient.PutAsync($"/api/Resolution/UpdateHeaderName", JsonContent.Create(args));
        }

        public Task<HttpResponseMessage> UpdateResolutionHeaderFullName(HeaderStringPropChangedEventArgs args)
        {
            return this._httpService.HttpClient.PutAsync($"/api/Resolution/UpdateHeaderFullName", JsonContent.Create(args));
        }
            

        public Task<HttpResponseMessage> UpdateResolutionHeaderTopic(HeaderStringPropChangedEventArgs args)
        {
            return this._httpService.HttpClient.PutAsync($"/api/Resolution/UpdateHeaderTopic", JsonContent.Create(args));
        }

        public Task<HttpResponseMessage> UpdateResolutionHeaderAgendaItem(HeaderStringPropChangedEventArgs args)
        {
            return this._httpService.HttpClient.PutAsync($"/api/Resolution/UpdateHeaderAgendaItem", JsonContent.Create(args));
        }

        public Task<HttpResponseMessage> UpdateResolutionHeaderSession(HeaderStringPropChangedEventArgs args)
        {
            return this._httpService.HttpClient.PutAsync($"/api/Resolution/UpdateHeaderSession", JsonContent.Create(args));
        }

        public Task<HttpResponseMessage> UpdateResolutionHeaderSubmitterName(HeaderStringPropChangedEventArgs args)
        {
            return this._httpService.HttpClient.PutAsync($"/api/Resolution/UpdateHeaderSubmitterName", JsonContent.Create(args));
        }
        public Task<HttpResponseMessage> UpdateResolutionHeaderCommitteeName(HeaderStringPropChangedEventArgs args)
        {
            return this._httpService.HttpClient.PutAsync($"/api/Resolution/UpdateHeaderCommitteeName", JsonContent.Create(args));
        }

        #endregion

        public Task<HttpResponseMessage> ResolutionAddPreambleParagraph(PreambleParagraphAddedEventArgs args)
        {
            return this._httpService.HttpClient.PostAsync($"/api/Resolution/AddPreambleParagraph", JsonContent.Create(args));
        }

        public Task<HttpResponseMessage> ResolutionPreambleParagraphTextChanged(PreambleParagraphTextChangedEventArgs args)
        {
            return this._httpService.HttpClient.PutAsync($"/api/Resolution/ChangePreambleParagraphText", JsonContent.Create(args));
        }

        public Task<HttpResponseMessage> ResolutionPreambleParagraphCommentTextChanged(PreambleParagraphCommentTextChangedEventArgs args)
        {
            return this._httpService.HttpClient.PutAsync($"/api/Resolution/ChangePreambleParagraphCommentText", JsonContent.Create(args));
        }

        public Task<HttpResponseMessage> DeleteResolutionPreambleParagraph(PreambleParagraphRemovedEventArgs args)
        {
            return this._httpService.HttpClient.PutAsync($"/api/Resolution/DeleteResolutionPreambleParagraph", JsonContent.Create(args));
        }

        public Task<HttpResponseMessage> UpdateOperativeParagraph(OperativeParagraphChangedEventArgs args)
        {
            return this._httpService.HttpClient.PutAsync($"/api/Resolution/UpdateOperativeParagraph", JsonContent.Create(args));
        }

        public Task<HttpResponseMessage> UpdateOperativeSection(OperativeSectionChangedEventArgs args)
        {
            return this._httpService.HttpClient.PutAsync($"/api/Resolution/UpdateOperativeSection", JsonContent.Create(args));
        }

        /// <summary>
        /// Checks if the Resolution Controller of the API is available.
        /// Will store the value for 30 Seconds and refresh when called 30 seconds after
        /// the last call. You can also set forceRefresh to true to force a new IsUp call to the API
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsOnline(bool forceRefresh = false)
        {
            if (forceRefresh || _isOnline == null || _lastOnlineChecked == null || (DateTime.Now - _lastOnlineChecked.Value).TotalSeconds > 30)
            {
                try
                {
                    var result = await this._httpService.HttpClient.GetAsync($"/api/Resolution/IsUp");
                    _isOnline = result.IsSuccessStatusCode;
                }
                catch (Exception)
                {
                    _isOnline = false;
                }
                
                _lastOnlineChecked = DateTime.Now;
            }
            return _isOnline.Value;
        }

        public async Task<bool> CanReadResolution(string resolutionId)
        {
            var client = await this._httpService.GetAuthClient();
            return await client.GetFromJsonAsync<bool>($"/api/Resolution/CanEditResolution?id={resolutionId}");
        }

        public async Task<bool> CanEditResolution(string resolutionId)
        {
            var client = await this._httpService.GetAuthClient();
            return await client.GetFromJsonAsync<bool>($"/api/Resolution/CanEditResolution?id={resolutionId}");
        }

        public async Task<List<ResolutionInfo>> GetStoredResolutions()
        {
            var resolutions = await this._localStorage.GetItemAsync<List<ResolutionInfo>>("munity_storedResolutions");
            if (resolutions == null) return new List<ResolutionInfo>();
            return resolutions;
        }

        public async Task<Resolution> GetResolution(string resolutionId)
        {
            var inLocalStorage = await GetStoredResolution(resolutionId);
            if (inLocalStorage != null) return inLocalStorage;
            return await GetResolutionFromServer(resolutionId);
        }

        public async Task<Resolution> CreateResolution(string title = "")
        {
            var resolution = new Resolution();
            resolution.Header.Topic = title;
            await StoreResolution(resolution);
            return resolution;
        }

        public async Task<bool> ResolutionExistsServerside(string id)
        {
            return await this._httpService.HttpClient.GetFromJsonAsync<bool>($"/api/Resolution/ResolutionExists?id={id}");
        }

        public async Task<Resolution> GetResolutionFromServer(string resolutionId)
        {
            try
            {
                var authedClient = await this._httpService.GetAuthClient();
                return await authedClient.GetFromJsonAsync<Resolution>($"/api/Resolution/GetResolution?id={resolutionId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> SyncResolutionWithServer(Resolution resolution)
        {
            try
            {
                var authedClient = await this._httpService.GetAuthClient();
                var content = JsonContent.Create(resolution);
                var msg = await authedClient.PatchAsync($"api/Resolution/UpdateResolution", content);
                return msg.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }


        private async Task StoreResolution(Resolution resolution)
        {
            await this._localStorage.SetItemAsync(GetResolutionLocalStorageName(resolution), resolution);
            // Can be performed async
            await UpdateStoredResolutionList(resolution);
        }

        private async Task UpdateStoredResolutionList(Resolution updatedResolution)
        {
            var storedResolutionInfos = await GetStoredResolutions();
            if (storedResolutionInfos == null) storedResolutionInfos = new List<ResolutionInfo>();
            var foundEntry = storedResolutionInfos.FirstOrDefault(n => n.ResolutionId == updatedResolution.ResolutionId);
            var info = (ResolutionInfo)updatedResolution;
            if (foundEntry != null)
            {
                foundEntry.LastChangedDate = info.LastChangedDate;
                foundEntry.ResolutionId = info.ResolutionId;
                foundEntry.Title = info.Title;
            }
            else
            {
                storedResolutionInfos.Add(info);
            }
            await this._localStorage.SetItemAsync("munity_storedResolutions", storedResolutionInfos);
        }

        private string GetResolutionLocalStorageName(Resolution resolution)
        {
            return GetResolutionLocalStorageName(resolution.ResolutionId);
        }

        private string GetResolutionLocalStorageName(string id)
        {
            return "mtr_" + id;
        }

        public async Task<Resolution> CreateOffline()
        {
            var resolution = new Resolution();
            resolution.Header.Topic = "No Title";
            await this.StoreResolution(resolution);
            return resolution;
        }

        /// <summary>
        /// Creates a public resolution and adds it to the editing resolution list.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public async Task<Resolution> CreatePublicResolution(string title)
        {
            var resolution = await this._httpService.HttpClient.GetFromJsonAsync<Resolution>($"/api/Resolution/CreatePublic?title={title}");
            if (resolution == null)
                return null;
            //await this.StoreResolution(resolution);
            return resolution;
        }

        public async Task<Resolution> GetStoredResolution(string id)
        {
            return await this._localStorage.GetItemAsync<Resolution>(GetResolutionLocalStorageName(id));
        }

        public async Task<HttpResponseMessage> UpdateResolutionPreambleParagraph(string resolutionid, PreambleParagraph paragraph, string tan)
        {
            var client = await this._httpService.GetAuthClient();
            var content = JsonContent.Create(paragraph);
            return  await client.PatchAsync($"/api/Resolution/UpdatePreambleParagraph?resolutionid={resolutionid}&tan={tan}", content);
        }

        public async Task<HttpResponseMessage> UpdateResolutionOperativeParagraph(string resolutionid, OperativeParagraph paragraph, string tan)
        {
            var client = await this._httpService.GetAuthClient();
            var content = JsonContent.Create(paragraph);
            return await client.PatchAsync($"/api/Resolution/UpdateOperativeParagraph?resolutionid={resolutionid}&tan={tan}", content);
        }

        public async Task<bool> CanUserPostAmendments(string resolutionId)
        {
            var client = await this._httpService.GetAuthClient();
            var result = await client.GetFromJsonAsync<bool>($"/api/Resolution/CanUserPostAmendments?resolutionId={resolutionId}");
            return result;
        }

        public async Task<HttpResponseMessage> PostDeleteAmendment(string resolutionId, DeleteAmendment amendment)
        {
            var client = await this._httpService.GetAuthClient();
            var content = JsonContent.Create(amendment);
            return await client.PostAsync($"/api/Resolution/PostDeleteAmendment?resolutionId={resolutionId}", content);
        }

        public async void SaveOfflineResolution(Resolution resolution)
        {
            await this.StoreResolution(resolution);
        }

        public string GenerateTan()
        {
            var chars = "abcdefghjklmopqrstuvwxyz0123456789";
            string tan = "";
            var rnd = new Random();
            for (int i=0;i<5;i++)
            {
                tan += chars[rnd.Next(0, chars.Length)];
            }
            return tan;
        }

        #region SignalR WebSocket

        public async Task<ViewModels.ResolutionViewModel> Subscribe(Resolution resolution)
        {
            var handler = await ViewModels.ResolutionViewModel.CreateViewModelOnline(resolution, this);
            var connId = handler.HubConnection.ConnectionId;
            await this._httpService.HttpClient.GetAsync($"/api/Resolution/SubscribeToResolution?resolutionid={resolution.ResolutionId}&connectionid={connId}");
            return handler;
        }

        #endregion

        [JSInvokable]
        public Task StorageHasChanged()
        {
            this.StorageChanged?.Invoke();
            return Task.FromResult("");
        }

        public ResolutionService(HttpService client, ILocalStorageService localStorage, IJSRuntime jSRuntime)
        {
            this._httpService = client;
            this._localStorage = localStorage;
            jSRuntime.InvokeVoidAsync("registerStorageListener", DotNetObjectReference.Create(this));
        }

    }
}
