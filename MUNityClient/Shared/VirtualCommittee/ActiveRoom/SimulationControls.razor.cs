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
            ListOfSpeakers,
        }

        [Parameter]
        public MUNityClient.Services.SocketHandlers.ListOfSpeakerSocketHandler SpeakerlistSocket
        {
            get;
            set;
        }

        [Parameter]
        public MUNityClient.ViewModels.SimulationViewModel ViewModel
        {
            get;
            set;
        }

        private TabPages _selectedTab = TabPages.None;
        private bool _hasNewPetition
        {
            get;
            set;
        } = false;
        

        

        private MUNity.Models.ListOfSpeakers.ListOfSpeakers _listOfSpeakers;
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        private void AddMeToListOfSpeakers()
        {
            if (SpeakerlistSocket == null)
                return;
            if (ViewModel?.MyRole != null)
            {
                listOfSpeakerService.AddSpeakerToList(SpeakerlistSocket.SourceList.ListOfSpeakersId, ViewModel.MyRole.Name, ViewModel.MyRole.Iso);
                return;
            }
            else
            {
                if (ViewModel?.Me != null)
                {
                    listOfSpeakerService.AddSpeakerToList(SpeakerlistSocket.SourceList.ListOfSpeakersId, ViewModel.Me.DisplayName, "");
                }
            }
        }

        private void AddMeToListOfQuestions()
        {
            if (SpeakerlistSocket == null)
                return;
            if (ViewModel?.MyRole != null)
            {
                listOfSpeakerService.AddQuestionToList(SpeakerlistSocket.SourceList.ListOfSpeakersId, ViewModel.MyRole.Name, ViewModel.MyRole.Iso);
                return;
            }
            else
            {
                if (ViewModel?.Me != null)
                {
                    listOfSpeakerService.AddQuestionToList(SpeakerlistSocket.SourceList.ListOfSpeakersId, ViewModel.Me.DisplayName, "");
                }
            }
        }
    }
}