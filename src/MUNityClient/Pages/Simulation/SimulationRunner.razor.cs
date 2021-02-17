using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using MUNityClient;
using MUNityClient.Shared;
using System.ComponentModel;

namespace MUNityClient.Pages.Simulation
{
    public partial class SimulationRunner
    {
        [Parameter]
        public string Id
        {
            get;
            set;
        }

        private MUNityClient.ViewModels.SimulationViewModel SimulationViewModelInstance { get; set; }
        private Services.SocketHandlers.ListOfSpeakerSocketHandler ListOfSpeakersInstance { get; set; }

        private MUNity.Models.ListOfSpeakers.ListOfSpeakers _listOfSpeakers;
        //private MUNity.Schema.Simulation.SimulationRoleItem _myRole;
        private string _listOfSpeakerId = "_loading_";
        private MUNityClient.Shared.VirtualCommittee.ActiveRoom.ActiveRoomLayoutWrapper _layoutWrapper;
        protected override async Task OnInitializedAsync()
        {
            _layoutWrapper = new Shared.VirtualCommittee.ActiveRoom.ActiveRoomLayoutWrapper();
            _layoutWrapper.PropertyChanged += LayoutChanged;
            int id = 0;
            if (int.TryParse(Id, out id))
            {
                SimulationViewModelInstance = await simulationService.Subscribe(id);

                AddHandlers(SimulationViewModelInstance);
                this._listOfSpeakerId = await this.simulationService.GetListOfSpeakerId(id);
                if (!string.IsNullOrEmpty(_listOfSpeakerId))
                {
                    this._listOfSpeakers = await listOfSpeakerService.GetFromApi(_listOfSpeakerId);
                    if (_listOfSpeakers != null)
                    {
                        ListOfSpeakersInstance = await listOfSpeakerService.Subscribe(_listOfSpeakers);
                        if (ListOfSpeakersInstance != null)
                        {
                            // To update/lock the Add Me to List of Speakers buttons when a list is closed or opened
                            ListOfSpeakersInstance.SpeakerListChanged += delegate { this.StateHasChanged(); };
                        }
                    }
                }

                this.StateHasChanged();
            }
        }

        private void AddMeToListOfSpeakers()
        {
            if (ListOfSpeakersInstance == null)
                return;
            if (SimulationViewModelInstance?.MyRole != null)
            {
                listOfSpeakerService.AddSpeakerToList(ListOfSpeakersInstance.SourceList.ListOfSpeakersId, SimulationViewModelInstance.MyRole.Name, SimulationViewModelInstance.MyRole.Iso);
                return;
            }
            else
            {
                if (SimulationViewModelInstance?.Me != null)
                {
                    listOfSpeakerService.AddSpeakerToList(ListOfSpeakersInstance.SourceList.ListOfSpeakersId, SimulationViewModelInstance.Me.DisplayName, "");
                }
            }
        }

        private void AddMeToListOfQuestions()
        {
            if (ListOfSpeakersInstance == null)
                return;
            if (SimulationViewModelInstance?.MyRole != null)
            {
                listOfSpeakerService.AddQuestionToList(ListOfSpeakersInstance.SourceList.ListOfSpeakersId, SimulationViewModelInstance.MyRole.Name, SimulationViewModelInstance.MyRole.Iso);
                return;
            }
            else
            {
                if (SimulationViewModelInstance?.Me != null)
                {
                    listOfSpeakerService.AddQuestionToList(ListOfSpeakersInstance.SourceList.ListOfSpeakersId, SimulationViewModelInstance.Me.DisplayName, "");
                }
            }
        }

        private void LayoutChanged(object sender, PropertyChangedEventArgs args)
        {
            this.StateHasChanged();
        }

        private void AddHandlers(MUNityClient.ViewModels.SimulationViewModel context)
        {
            context.PhaseChanged += OnPhaseChanged;
            context.CurrentResolutionChanged += delegate { this.StateHasChanged(); };
            context.HubConnection.Closed += (ex) =>
            {
                Console.WriteLine($"{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")} Verbindung beendet");
                if (ex != null)
                    Console.WriteLine(ex.Message);
                this.StateHasChanged();
                return null;
            };
            context.HubConnection.Reconnected += (ex) =>
            {
                Console.WriteLine($"{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")} Reconnected");
                if (ex != null)
                    Console.WriteLine(ex);
                this.StateHasChanged();
                return null;
            };
            context.HubConnection.Reconnecting += (ex) =>
            {
                Console.WriteLine($"{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")} Reconnecting");
                if (ex != null)
                    Console.WriteLine(ex.Message);
                this.StateHasChanged();
                return null;
            };
        }

        private void OnPhaseChanged(int sender, MUNity.Schema.Simulation.GamePhases phase)
        {
            if (phase == MUNity.Schema.Simulation.GamePhases.Lobby)
            {
                navigationManager.NavigateTo($"/sim/lobby/{Id}");
            }
        }

        private async void InitListOfSpeakers()
        {
            if (this.SimulationViewModelInstance?.Simulation != null)
            {
                var list = await this.simulationService.InitListOfSpeakers(this.SimulationViewModelInstance.Simulation.SimulationId);
                if (list != null)
                {
                    this._listOfSpeakerId = list.ListOfSpeakersId;
                    ListOfSpeakersInstance = await listOfSpeakerService.Subscribe(_listOfSpeakers);
                    if (ListOfSpeakersInstance != null)
                    {
                        // To update/lock the Add Me to List of Speakers buttons when a list is closed or opened
                        ListOfSpeakersInstance.SpeakerListChanged += delegate { this.StateHasChanged(); };
                    }
                    this.StateHasChanged();
                }
            }
        }
    }
}