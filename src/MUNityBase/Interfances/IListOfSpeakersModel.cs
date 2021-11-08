using MUNity.Base;
using System;

namespace MUNityBase.Interfances
{
    public interface IListOfSpeakersModel
    {
        string ListOfSpeakersId { get; set; }

        string PublicId { get; set; }

        string Name { get; set; }

        ESpeakerListStatus Status { get; set; }

        TimeSpan SpeakerTime { get; set; }

        TimeSpan QuestionTime { get; set; }

        TimeSpan PausedSpeakerTime { get; set; }

        TimeSpan PausedQuestionTime { get; set; }

        bool ListClosed { get; set; }

        bool QuestionsClosed { get; set; }

        DateTime StartSpeakerTime { get; set; }

        DateTime StartQuestionTime { get; set; }
    }
}
