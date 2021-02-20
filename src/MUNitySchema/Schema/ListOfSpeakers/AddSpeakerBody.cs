using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.ListOfSpeakers
{
    public class AddSpeakerBody : ListOfSpeakersRequest
    {
        public Models.ListOfSpeakers.Speaker Speaker { get; set; }
    }
}
