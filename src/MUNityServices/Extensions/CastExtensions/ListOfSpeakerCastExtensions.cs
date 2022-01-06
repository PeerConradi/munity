using MUNity.Database.Models.LoS;
using MUNity.ViewModels.ListOfSpeakers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Services.Extensions.CastExtensions
{
    public static class ListOfSpeakerCastExtensions
    {
        public static ListOfSpeakersViewModel ToViewModel(this ListOfSpeakers source)
        {
            var model = new ViewModels.ListOfSpeakers.ListOfSpeakersViewModel()
            {
                AllSpeakers = source.AllSpeakers.Select(a => new ViewModels.ListOfSpeakers.SpeakerViewModel()
                {
                    Id = a.Id,
                    Iso = a.Iso,
                    Mode = a.Mode,
                    Name = a.Name,
                    OrdnerIndex = a.OrdnerIndex,
                }).ToObservableCollection(),
                ListClosed = source.ListClosed,
                Name = source.Name,
                ListOfSpeakersId = source.ListOfSpeakersId,
                PausedQuestionTime = source.PausedSpeakerTime,
                PausedSpeakerTime = source.PausedSpeakerTime,
                PublicId = source.PublicId,
                QuestionsClosed = source.QuestionsClosed,
                SpeakerTime = source.SpeakerTime,
                QuestionTime = source.QuestionTime,
                StartQuestionTime = source.StartQuestionTime,
                StartSpeakerTime = source.StartSpeakerTime,
                Status = source.Status,
            };
            return model;
        }
    }
}
