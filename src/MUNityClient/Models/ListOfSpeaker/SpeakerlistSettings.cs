using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityClient.Models.ListOfSpeaker
{
    public class SpeakerlistSettings
    {
        public string Speakertime
        {
            get;
            set;
        }

        public string Questiontime
        {
            get;
            set;
        }

        public SpeakerlistSettings()
        {
        }

        public SpeakerlistSettings(MUNity.Models.ListOfSpeakers.ListOfSpeakers source)
        {
            Speakertime = source.SpeakerTime.ToString(@"mm\:ss");
            Questiontime = source.QuestionTime.ToString(@"mm\:ss");
        }
    }
}
