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

namespace MUNityClient.Shared.VirtualCommittee.ActiveRoom
{
    public partial class SimulationControls
    {
        private enum TabPages
        {
            None,
            GeneralControls,
            ListOfSpeakers,
            Voting,
            Presents,
            Petitions
        }

        [Parameter]
        public MUNityClient.Services.SocketHandlers.ListOfSpeakerSocketHandler SpeakerlistSocket
        {
            get;
            set;
        }

        [Parameter]
        public MUNityClient.ViewModel.SimulationViewModel SimulationContext
        {
            get;
            set;
        }

        private TabPages _selectedTab = TabPages.None;
        private bool _hasNewPetition
        {
            get;
            set;
        }

        = false;
        private bool _hasNewVotes
        {
            get;
            set;
        }

        = false;
        private MUNity.Schema.Simulation.CreatedVoteModel _lastCreatedVote
        {
            get;
            set;
        }

        = null;
        private MUNity.Models.ListOfSpeakers.ListOfSpeakers _listOfSpeakers;
        protected override void OnInitialized()
        {
            if (SimulationContext != null)
            {
                SimulationContext.VoteCreated += VoteCreated;
            }

            base.OnInitialized();
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        private void VoteCreated(object sender, MUNity.Schema.Simulation.CreatedVoteModel args)
        {
            _hasNewVotes = true;
            _lastCreatedVote = args;
            StateHasChanged();
        }

        private async Task VotePro()
        {
            if (_lastCreatedVote != null)
            {
                await this.simulationService.Vote(SimulationContext.Simulation.SimulationId, _lastCreatedVote.CreatedVoteModelId, 0);
                _lastCreatedVote = null;
                _hasNewVotes = false;
                _selectedTab = TabPages.None;
            }
        }

        private async Task VoteCon()
        {
            if (_lastCreatedVote != null)
            {
                await this.simulationService.Vote(SimulationContext.Simulation.SimulationId, _lastCreatedVote.CreatedVoteModelId, 1);
                _lastCreatedVote = null;
                _hasNewVotes = false;
                _selectedTab = TabPages.None;
            }
        }

        private async Task VoteAbstention()
        {
            if (_lastCreatedVote != null)
            {
                await this.simulationService.Vote(SimulationContext.Simulation.SimulationId, _lastCreatedVote.CreatedVoteModelId, 2);
                _lastCreatedVote = null;
                _hasNewVotes = false;
                _selectedTab = TabPages.None;
            }
        }

        private void AddMeToListOfSpeakers()
        {
            if (SpeakerlistSocket == null)
                return;
            if (SimulationContext?.MyRole != null)
            {
                listOfSpeakerService.AddSpeakerToList(SpeakerlistSocket.SourceList.ListOfSpeakersId, SimulationContext.MyRole.Name, SimulationContext.MyRole.Iso);
                return;
            }
            else
            {
                if (SimulationContext?.Me != null)
                {
                    listOfSpeakerService.AddSpeakerToList(SpeakerlistSocket.SourceList.ListOfSpeakersId, SimulationContext.Me.DisplayName, "");
                }
            }
        }

        private void AddMeToListOfQuestions()
        {
            if (SpeakerlistSocket == null)
                return;
            if (SimulationContext?.MyRole != null)
            {
                listOfSpeakerService.AddQuestionToList(SpeakerlistSocket.SourceList.ListOfSpeakersId, SimulationContext.MyRole.Name, SimulationContext.MyRole.Iso);
                return;
            }
            else
            {
                if (SimulationContext?.Me != null)
                {
                    listOfSpeakerService.AddQuestionToList(SpeakerlistSocket.SourceList.ListOfSpeakersId, SimulationContext.Me.DisplayName, "");
                }
            }
        }
    }
}