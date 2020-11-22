using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Schema.Response.Conference
{
    public class ConferenceSmallInfo
    {
        public string ConferenceId { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public ConferenceSmallInfo(Models.Conference.Conference conference)
        {
            this.ConferenceId = conference.ConferenceId;
            this.Name = conference.Name;
            this.FullName = conference.FullName;
        }

        public static implicit operator ConferenceSmallInfo(Models.Conference.Conference conference)
        {
            return new ConferenceSmallInfo(conference);
        }
    }
}
