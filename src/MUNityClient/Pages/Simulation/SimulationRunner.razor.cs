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
using MUNityClient.Models.ListOfSpeaker;

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

        private string AddToListOfSpeakerPhrase
        {
            get
            {
                if (ListOfSpeakersInstance == null)
                    return "Keine Redeliste gefunden";
                if (ListOfSpeakersInstance.SourceList == null)
                    return "Redeliste fehlerhaft geladen";
                if (ListOfSpeakersInstance.SourceList.ListClosed)
                    return "Redeliste ist geschlossen";
                if (SimulationViewModelInstance.MySlot == null)
                    return "Diese funktion ist nur m�glich, wenn eine entsprechende Rolle vorliegt.";
                if (ListOfSpeakersInstance.SourceList.Speakers.Any(n => n.Name == SimulationViewModelInstance.MySlot.RoleName))
                    return $"Es befindet sich bereits ein Eintrag {SimulationViewModelInstance.MySlot.RoleName} auf der Redeliste";
                return $"Als {SimulationViewModelInstance.MySlot.RoleName} auf die Redeliste setzen.";
            }
        }

        private string AddToQuestionsPhrase
        {
            get
            {
                if (ListOfSpeakersInstance == null)
                    return "Keine Redeliste gefunden.";
                if (ListOfSpeakersInstance.SourceList == null)
                    return "Redeliste fehlerhaft geladen.";
                if (ListOfSpeakersInstance.SourceList.QuestionsClosed)
                    return "Fragen und Kurzbemerkungen sind geschlossen.";
                if (SimulationViewModelInstance.MySlot == null)
                    return "Diese funktion ist nur m�glich, wenn eine entsprechende Rolle vorliegt.";
                if (ListOfSpeakersInstance.SourceList.Questions.Any(n => n.Name == SimulationViewModelInstance.MySlot.RoleName))
                    return $"Es befindet sich bereits ein Eintrag {SimulationViewModelInstance.MySlot.RoleName} auf den Fragen und Kurzbemerkungen.";
                return $"Als {SimulationViewModelInstance.MySlot.RoleName} auf die Fragen und Kurzbemerkungen setzen.";
            }
        }

        private bool AllowedToAddToSpeakers
        {
            get
            {
                if (ListOfSpeakersInstance == null)
                    return false;
                if (ListOfSpeakersInstance.SourceList == null)
                    return false;
                if (ListOfSpeakersInstance.SourceList.ListClosed)
                    return false;
                if (SimulationViewModelInstance.MySlot == null)
                    return false;
                if (ListOfSpeakersInstance.SourceList.Speakers.Any(n => n.Name == SimulationViewModelInstance.MySlot.RoleName))
                    return false;
                return true;
            }
        }

        private bool AllowedToAddToQuestions
        {
            get
            {
                if (ListOfSpeakersInstance == null)
                    return false;
                if (ListOfSpeakersInstance.SourceList == null)
                    return false;
                if (ListOfSpeakersInstance.SourceList.QuestionsClosed)
                    return false;
                if (SimulationViewModelInstance.MySlot == null)
                    return false;
                if (ListOfSpeakersInstance.SourceList.Questions.Any(n => n.Name == SimulationViewModelInstance.MySlot.RoleName))
                    return false;
                return true;
            }
        }

        private MUNityClient.ViewModels.SimulationViewModel SimulationViewModelInstance { get; set; }
        private ViewModels.ListOfSpeakerViewModel ListOfSpeakersInstance { get; set; }

        private MUNity.Models.ListOfSpeakers.ListOfSpeakers _listOfSpeakers;
        //private MUNity.Schema.Simulation.SimulationRoleItem _myRole;
        private string _listOfSpeakerId = "_loading_";
        private MUNityClient.Shared.VirtualCommittee.ActiveRoom.ActiveRoomLayoutWrapper _layoutWrapper;
        protected override async Task OnInitializedAsync()
        {
            _layoutWrapper = new Shared.VirtualCommittee.ActiveRoom.ActiveRoomLayoutWrapper();
            
            _layoutWrapper.PropertyChanged += LayoutChanged;
            int simulationId = 0;
            if (int.TryParse(Id, out simulationId))
            {
                var tabId = await this.simulationService.GetLastOpenedTab(simulationId);
                _layoutWrapper.MainContent = (Shared.VirtualCommittee.ActiveRoom.ActiveRoomLayoutWrapper.MainContents)tabId;
                SimulationViewModelInstance = await simulationService.Subscribe(simulationId);

                AddHandlers(SimulationViewModelInstance);
                this._listOfSpeakerId = await this.simulationService.GetListOfSpeakerId(simulationId);
                Console.WriteLine("list of speakers id: " + this._listOfSpeakerId);
                if (!string.IsNullOrEmpty(_listOfSpeakerId))
                {
                    this._listOfSpeakers = await listOfSpeakerService.GetFromApi(_listOfSpeakerId);
                    if (_listOfSpeakers != null)
                    {
                        ListOfSpeakersInstance = await MUNityClient.ViewModels.ListOfSpeakerViewModel.GetFromOnline(listOfSpeakerService, _listOfSpeakerId);
                        if (ListOfSpeakersInstance != null)
                        {
                            ListOfSpeakersInstance.Handler.SpeakerStateChanged += delegate { this.StateHasChanged(); };
                            ListOfSpeakersInstance.Handler.QuestionsStateChanged += delegate { this.StateHasChanged(); };
                        }
                    }
                }

                this.StateHasChanged();
            }
        }

        private async Task AddMeToListOfSpeakers()
        {
            if (ListOfSpeakersInstance == null)
                return;

            if (SimulationViewModelInstance != null && !string.IsNullOrEmpty(SimulationViewModelInstance.MySlot.RoleName))
            {
                var body = new SpeakerToAdd()
                {
                    Iso = SimulationViewModelInstance.MySlot.RoleIso,
                    Name = SimulationViewModelInstance.MySlot.RoleName
                };
                await ListOfSpeakersInstance.Handler.AddSpeaker(body);
                return;
            }
            else
            {
                var body = new SpeakerToAdd()
                {
                    Iso = "un",
                    Name = SimulationViewModelInstance.MySlot.DisplayName
                };
                await ListOfSpeakersInstance.Handler.AddSpeaker(body);
                return;
            }
        }

        private async Task AddMeToListOfQuestions()
        {
            if (ListOfSpeakersInstance == null)
                return;

            

            if (SimulationViewModelInstance != null && !string.IsNullOrEmpty(SimulationViewModelInstance.MySlot.RoleName))
            {
                var body = new SpeakerToAdd()
                {
                    Iso = SimulationViewModelInstance.MySlot.RoleIso,
                    Name = SimulationViewModelInstance.MySlot.RoleName
                };
                await ListOfSpeakersInstance.Handler.AddQuestion(body);
                return;
            }
            else
            {
                var body = new SpeakerToAdd()
                {
                    Iso = "un",
                    Name = SimulationViewModelInstance.MySlot.DisplayName
                };
                await ListOfSpeakersInstance.Handler.AddQuestion(body);
                return;
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
                    this.StateHasChanged();
                }
            }
        }
    }
}