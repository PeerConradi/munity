using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Conference
{
    public class ParticipatingConferenceInfo
    {
        public string ConferenceId { get; set; }

        public string ConferenceShort { get; set; }

        public string ConferenceFullName { get; set; }

        public bool IsTeamMember { get; set; }
    }
}
