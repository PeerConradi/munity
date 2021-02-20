using Microsoft.AspNetCore.SignalR.Client;
using MUNity.Models.ListOfSpeakers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityClient.ViewModels
{
    /// <summary>
    /// The List of Speaker View Model handles all the logic and importand part of a List of Speakers.
    /// It contains the List Model iside the SourceList-Property <see cref="ListOfSpeakerViewModel.SourceList"/>.
    /// </summary>
    public class ListOfSpeakerViewModel
    {

        public ListOfSpeakers SourceList { get; private set; }

        public Services.ListOfSpeakerService Service { get; private set; }
            
        public bool IsOnline { get; private set; }

        public ViewModelLogic.IListOfSpeakerHandler Handler { get; set; }

        public bool LowOnSpeakerTime => this.SourceList.RemainingSpeakerTime.TotalSeconds < 11;

        public bool OutOfSpeakerTime => this.SourceList.RemainingSpeakerTime.TotalSeconds < 0;

        public bool LowOnQuestionTime => this.SourceList.RemainingQuestionTime.TotalSeconds < 11;

        public bool OutOfQuestionTime => this.SourceList.RemainingQuestionTime.TotalSeconds < 0;

        private ListOfSpeakerViewModel(ListOfSpeakers listOfSpeakers, Services.ListOfSpeakerService service)
        {
            SourceList = listOfSpeakers;
            this.Service = service;
        }

        private void ListOfSpeakerSocketHandler_SpeakerListChanged(object sender, ListOfSpeakers e)
        {
            if (e.ListOfSpeakersId == SourceList.ListOfSpeakersId)
            {
                SourceList = e;
            }
        }

        public static async Task<ListOfSpeakerViewModel> CreateViewModel(Services.ListOfSpeakerService service, string listId)
        {
            var isOnline = await service.IsListOfSpeakersOnline(listId);
            if (isOnline)
                return await CreateFromOnline(service, listId);
            else
                return await CreateFromOffline(service, listId);
        }

        public static async Task<ListOfSpeakerViewModel> CreateFromOffline(Services.ListOfSpeakerService service, string listId)
        {
            var list = await service.GetListOfSpeakersOffline(listId);
            var mdl = new ListOfSpeakerViewModel(list, service);
            mdl.Handler = new ViewModelLogic.ListOfSpeakerOfflineHandler();
            return mdl;
        }

        public static async Task<ListOfSpeakerViewModel> CreateFromOnline(Services.ListOfSpeakerService service, string listId)
        {
            var list = await service.GetFromApi(listId);
            var mdl = new ListOfSpeakerViewModel(list, service);
            mdl.Handler = new ViewModelLogic.ListOfSpeakerOnlineHandler();
            await mdl.Handler.Init(mdl);
            return mdl;
        }



        
    }
}
