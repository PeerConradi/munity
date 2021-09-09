using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNity.Models.ListOfSpeakers;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Components;
using MUNity.Hubs;
using MUNity.Extensions.LoSExtensions;
using MUNity.Schema.ListOfSpeakers;
using MUNityCore.ViewModel;

namespace MUNity.Services
{
    public class SpeakerlistHubService : IDisposable
    {
        private List<SpeakerlistViewModel> _viewModels = new List<SpeakerlistViewModel>();

        private SpeakerlistService _speakerListService;

        private readonly NavigationManager _navigationManager;

        public async Task<SpeakerlistViewModel> GetSpeakerlistViewModel(string speakerlistId)
        {
            var list = _viewModels.FirstOrDefault(n => n.SpeakerlistId == speakerlistId);
            if (list != null)
                return list;

            var listModel = this._speakerListService.GetSpeakerlist(speakerlistId);
            list = new SpeakerlistViewModel(listModel, $"{_navigationManager.BaseUri}slsocket");
            await list.Start();
            _viewModels.Add(list);
            return list;
        }

        public void Dispose()
        {
            foreach(var viewModel in _viewModels)
            {
                viewModel.Dispose();
            }
        }

        public SpeakerlistHubService(SpeakerlistService speakerListService, NavigationManager navManager)
        {
            this._speakerListService = speakerListService;
            this._navigationManager = navManager;
        }
        

    }
}