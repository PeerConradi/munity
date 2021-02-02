using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Conference
{
    /// <summary>
    /// A schema the MUNityAPI will give you when requesting information about a conference.
    /// </summary>
    public class ConferenceInformation
    {
        /// <summary>
        /// The id of the conference.
        /// </summary>
        public string ConferenceId { get; set; }

        /// <summary>
        /// the name of the conference. For example: MUN Schleswig-Holstein 2021.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The full name of the conference, for example: Model United Nations Schleswig-Holstein 2021
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// The short name of the conference, for example: MUN-SH 2021.
        /// </summary>
        public string ConferenceShort { get; set; }

        /// <summary>
        /// The start Date of the conference.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The end Date of the conference.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// The id of the parent project of this conference. Every conference is inside a project for example
        /// Model United Nations Schleswig-Holstein 2021 is part of the project Model United Nations Schleswig-Holstein.
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// A list with basic information about all the committees inside the conference.
        /// </summary>
        public IEnumerable<CommitteeSmallInfo> Committees { get; set; }
    }
}
