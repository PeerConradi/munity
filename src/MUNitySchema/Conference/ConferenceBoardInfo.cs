using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Conference
{
    public class ConferenceBoardInfo
    {
        public string ConferenceId { get; set; }

        public string ConferenceName { get; set; }

        public string ConferenceFullName { get; set; }

        public string ConferenceShort { get; set; }

        public bool UserIsParticipating { get; set; }

        public bool UserIsTeamMember { get; set; }

        public bool UserIsOwner { get; set; }

        public string DashboardText { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
