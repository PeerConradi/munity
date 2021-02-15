using Microsoft.AspNetCore.SignalR.Client;
using MUNity.Models.ListOfSpeakers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityClient.Services.SocketHandlers
{
    /// <summary>
    /// A Handler for the SignalR Socket in Context of the Simulation.
    /// 
    /// Maps the incoming SingalR Signals and maps them to Events.
    /// </summary>
    public class ListOfSpeakerSocketHandler
    {

        public ListOfSpeakers SourceList { get; private set; }
            

        public event EventHandler<int> QuestionTimerStarted;

        public event EventHandler<ListOfSpeakers> SpeakerListChanged;

        public event EventHandler<int> SpeakerTimerStarted;

        public event EventHandler TimerStopped;

        public HubConnection HubConnection { get; set; }

        private ListOfSpeakerSocketHandler(ListOfSpeakers listOfSpeakers)
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

        public static async Task<ListOfSpeakerSocketHandler> CreateHandler(ListOfSpeakers listOfSpeakers)
        {
            var instance = new ListOfSpeakerSocketHandler(listOfSpeakers);
            await instance.HubConnection.StartAsync();
            return instance;
        }

        
    }
}
