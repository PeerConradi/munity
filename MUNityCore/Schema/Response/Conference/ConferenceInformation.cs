using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Schema.Response.Conference
{
    public class ConferenceInformation
    {
        public string ConferenceId { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string ConferenceShort { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string ProjectId { get; set; }

        public IEnumerable<CommitteeSmallInfo> CommitteeIds { get; set; }

        public ConferenceInformation(Models.Conference.Conference conference)
        {
            this.ConferenceId = conference.ConferenceId;
            this.Name = conference.Name;
            this.FullName = conference.FullName;
            this.ConferenceShort = conference.ConferenceShort;
            this.StartDate = conference.StartDate;
            this.EndDate = conference.EndDate;
            this.ProjectId = conference.ConferenceProject.ProjectId;
            this.CommitteeIds = conference.Committees.Cast<CommitteeSmallInfo>();
        }

        public static implicit operator ConferenceInformation (Models.Conference.Conference conference)
        {
            return new ConferenceInformation(conference);
        }
    }
}
