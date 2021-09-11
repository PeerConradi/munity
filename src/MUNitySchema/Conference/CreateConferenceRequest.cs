using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MUNity.Schema.Conference
{
    /// <summary>
    /// The request body to create a new conference.
    /// </summary>
    public class CreateConferenceRequest
    {
        /// <summary>
        /// Every conference needs to be part of a project. for example Model United Nations Schleswig-Holstein 2021 would be part
        /// of the project Model United Nations Schleswig-Holstein.
        /// You could also create a project named: "Model United Nations in Schools" and then create
        /// a conference named: MUN2021@SchoolX and make it part of the project "Model United Nations in Schools".
        /// </summary>
        [Required]
        public string ProjectId { get; set; }

        /// <summary>
        /// The Name of the conference for example MUN Schleswig-Holstein 2021.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The full name of the conference for example Model United Nations Schleswig-Holstein 2021.
        /// </summary>
        [Required]
        public string FullName { get; set; }

        /// <summary>
        /// A short name of the conference. Like: MUN-SH 2021
        /// </summary>
        [Required]
        public string ConferenceShort { get; set; }

        /// <summary>
        /// The start Date of the Conference. Note that the Schema should be 08/18/2018 07:22:16, 08/18/2018, 2018-08-18T07:22:16.0000000Z or any other
        /// allowed format you can find here: https://docs.microsoft.com/de-de/dotnet/api/system.datetime.parse?view=net-5.0#
        /// <see cref="DateTime.Parse(string)"/>
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// The end Date of the Conference. Note that the Schema should be 08/18/2018 07:22:16, 08/18/2018, 2018-08-18T07:22:16.0000000Z or any other
        /// allowed format you can find here: https://docs.microsoft.com/de-de/dotnet/api/system.datetime.parse?view=net-5.0#
        /// <see cref="DateTime.Parse(string)"/>
        /// </summary>
        public string EndDate { get; set; }
    }

    public class CreateConferenceResponse
    {
        public enum CreateConferecenStatuses
        {
            Success,
            Error,
            NameTaken,
            ShortTaken,
            NoPermission,
            ProjectNotFound
        }

        public CreateConferecenStatuses Status { get; set; }

        public string ConferenceId { get; set; }
    }
}
