using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.ListOfSpeakers
{
    public class AddSpeakerBody : ListOfSpeakersRequest
    {
        public string Name { get; set; }

        public string Iso { get; set; }
    }
}
