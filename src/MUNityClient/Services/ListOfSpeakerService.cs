using Blazored.LocalStorage;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using MUNity.Models.ListOfSpeakers;
using System.Net.Http.Json;
using System.Net.Http;

namespace MUNityClient.Services
{
    public class ListOfSpeakerService
    {

        public delegate void OnStorageChanged();

        public event OnStorageChanged StorageChanged;

        private readonly HttpService _httpService;

        private readonly ILocalStorageService _localStorage;

        public async Task<ListOfSpeakers> CreateListOfSpeakers()
        {
            var listOfSpeakers = new ListOfSpeakers();
            await StoreListOfSpeakers(listOfSpeakers);
            return listOfSpeakers;
        }

        public async Task<ListOfSpeakers> GetListOfSpeakersOffline(string id)
        {
            return await this._localStorage.GetItemAsync<ListOfSpeakers>(ListOfSpeakerIdInStorage(id));
        }

        public async Task StoreListOfSpeakers(ListOfSpeakers list)
        {
            await this._localStorage.SetItemAsync(ListOfSpeakerIdInStorage(list.ListOfSpeakersId), list);
        }

        private string ListOfSpeakerIdInStorage(string id) => "mtlos_" + id;

        public async Task<ListOfSpeakers> GetFromApi(string id)
        {
            var list =  await this._httpService.HttpClient.GetFromJsonAsync<ListOfSpeakers>($"/api/Speakerlist/GetSpeakerlist?id={id}");
            await StoreListOfSpeakers(list);
            return list;
        }

        /// <summary>
        /// Checks if a List of speakers exists online.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> IsListOfSpeakersOnline(string id)
        {
            return await this._httpService.HttpClient.GetFromJsonAsync<bool>($"/api/Speakerlist/IsSpeakerlistOnline?id={id}");
        }

        public async void SyncSpeakerlist(ListOfSpeakers list)
        {
            var content = JsonContent.Create(list);
            var contentString = content.ToString();
            await this._httpService.HttpClient.PutAsync($"/api/Speakerlist/SyncSpeakerlist", content);
        }

        public async void AddSpeakerToList(string listOfSpeakersId, string name, string iso)
        {
            var mdl = new Speaker();
            mdl.Iso = iso;
            mdl.Name = name;
            await this._httpService.HttpClient.PostAsync($"/api/Speakerlist/AddSpeakerModelToList?listid={listOfSpeakersId}", JsonContent.Create(mdl));
        }

        public async void AddQuestionToList(string listOfSpeakersId, string name, string iso)
        {
            var mdl = new Speaker();
            mdl.Iso = iso;
            mdl.Name = name;
            await this._httpService.HttpClient.PostAsync($"/api/Speakerlist/AddQuestionModelToList?listid={listOfSpeakersId}", JsonContent.Create(mdl));
        }

        public Task<HttpResponseMessage> Subscribe(string listId, string connectionId)
        {
            return this._httpService.HttpClient.GetAsync($"/api/Speakerlist/SubscribeToList?listId={listId}&connectionid={connectionId}");
        }

        public Task<HttpResponseMessage> CreateOnline()
        {
            return this._httpService.HttpClient.GetAsync($"/api/Speakerlist/CreateListOfSpeaker");
        }

        [JSInvokable]
        public Task StorageHasChanged()
        {
            this.StorageChanged?.Invoke();
            return Task.FromResult("");
        }

        public ListOfSpeakerService(HttpService httpService, ILocalStorageService localStorage, IJSRuntime jsRuntime)
        {
            this._httpService = httpService;
            this._localStorage = localStorage;
            jsRuntime.InvokeVoidAsync("registerStorageListener", DotNetObjectReference.Create(this));
        }
    }
}
