using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.ListOfSpeakers
{
    public class RemoveSpeakerBody : ListOfSpeakersRequest
    {
        public string SpeakerId { get; set; }
    }
}
