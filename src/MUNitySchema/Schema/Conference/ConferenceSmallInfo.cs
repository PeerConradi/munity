using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Conference
{

    /// <summary>
    /// A very small schema that the MUNity API will return when asking for a list of conferences.
    /// </summary>
    public class ConferenceSmallInfo
    {
        /// <summary>
        /// The id of the conference.
        /// </summary>
        public string ConferenceId { get; set; }

        /// <summary>
        /// The Name like: MUN Schleswig-Holstein 2021
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The full name like: Model United Nations Schleswig-Holstein 2021.
        /// </summary>
        public string FullName { get; set; }
    }
}
