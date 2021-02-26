using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.ListOfSpeakers
{
    public class SetListSettingsBody : ListOfSpeakersRequest, IListTimeSettings
    {
        public string SpeakerTime { get; set; }

        public string QuestionTime { get; set; }
    }

    public interface IListTimeSettings
    {
        string SpeakerTime { get; set; }

        string QuestionTime { get; set; }
    }
}
