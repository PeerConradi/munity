using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Project
{
    /// <summary>
    /// The basic schema the MUNityAPI will sent when requestiong information about a project.
    /// </summary>
    public class ProjectInformation
    {
        /// <summary>
        /// The id of the project.
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// The name for example: Model United Nations Schleswig-Holstein.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// The short name of the project for example: MUN-SH
        /// </summary>
        public string ProjectShort { get; set; }

        /// <summary>
        /// The id of the organization that is hosting this project.
        /// </summary>
        public string OrganizationId { get; set; }

        /// <summary>
        /// A list of conferences that is contained inside of this project.
        /// </summary>
        public IEnumerable<Conference.ConferenceSmallInfo> Conferences { get; set; }
    }
}
