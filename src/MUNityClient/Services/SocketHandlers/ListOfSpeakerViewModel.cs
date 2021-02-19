using Microsoft.AspNetCore.SignalR.Client;
using MUNity.Models.ListOfSpeakers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityClient.Services.SocketHandlers
{
    /// <summary>
    /// The List of Speaker View Model handles all the logic and importand part of a List of Speakers.
    /// It contains the List Model iside the SourceList-Property <see cref="ListOfSpeakerViewModel.SourceList"/>.
    /// </summary>
    public class ListOfSpeakerViewModel
    {

        public ListOfSpeakers SourceList { get; private set; }
            

        public event EventHandler<int> QuestionTimerStarted;

        public event EventHandler<ListOfSpeakers> SpeakerListChanged;

        public event EventHandler<int> SpeakerTimerStarted;

        public event EventHandler TimerStopped;

        public HubConnection HubConnection { get; set; }

        private ListOfSpeakerViewModel(ListOfSpeakers listOfSpeakers)
        {
            SourceList = listOfSpeakers;
            
            HubConnection = new HubConnectionBuilder().WithUrl($"{Program.API_URL}/slsocket").Build();

            SpeakerListChanged += ListOfSpeakerSocketHandler_SpeakerListChanged;

            HubConnection.On<int>(nameof(MUNity.Hubs.ITypedListOfSpeakerHub.QuestionTimerStarted), (seconds) => QuestionTimerStarted?.Invoke(this, seconds));
            HubConnection.On<ListOfSpeakers>(nameof(MUNity.Hubs.ITypedListOfSpeakerHub.SpeakerListChanged), (list) => SpeakerListChanged?.Invoke(this, list));
            HubConnection.On<int>(nameof(MUNity.Hubs.ITypedListOfSpeakerHub.SpeakerTimerStarted), (seconds) => SpeakerTimerStarted.Invoke(this, seconds));
            HubConnection.On<string>(nameof(MUNity.Hubs.ITypedListOfSpeakerHub.TimerStopped),(s) => TimerStopped?.Invoke(this, new EventArgs()));
        }

        private void ListOfSpeakerSocketHandler_SpeakerListChanged(object sender, ListOfSpeakers e)
        {
            if (e.ListOfSpeakersId == SourceList.ListOfSpeakersId)
            {
                SourceList = e;
            }
        }

        public static async Task<ListOfSpeakerViewModel> CreateHandler(ListOfSpeakers listOfSpeakers)
        {
            var instance = new ListOfSpeakerViewModel(listOfSpeakers);
            await instance.HubConnection.StartAsync();
            return instance;
        }

        
    }
}
