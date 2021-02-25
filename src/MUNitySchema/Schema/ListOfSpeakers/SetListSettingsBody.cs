using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.ListOfSpeakers
{
    public class SetListSettingsBody : ListOfSpeakersRequest, IListTimeSettings
    {
        public TimeSpan SpeakerTime { get; set; }

        public TimeSpan QuestionTime { get; set; }
    }

    public interface IListTimeSettings
    {
        TimeSpan SpeakerTime { get; set; }

        TimeSpan QuestionTime { get; set; }
    }
}
