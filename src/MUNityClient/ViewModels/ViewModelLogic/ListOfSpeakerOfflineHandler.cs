using MUNity.Models.ListOfSpeakers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityClient.ViewModels.ViewModelLogic
{
    public class ListOfSpeakerOfflineHandler : IListOfSpeakerHandler
    {
        public event EventHandler<int> QuestionTimerStarted;
        public event EventHandler<ListOfSpeakers> SpeakerListChanged;
        public event EventHandler<int> SpeakerTimerStarted;
        public event EventHandler TimerStopped;

        public Task Init(ListOfSpeakerViewModel viewModel)
        {
            throw new NotImplementedException();
        }
    }
}
