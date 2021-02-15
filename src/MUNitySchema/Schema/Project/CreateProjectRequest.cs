using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MUNity.Schema.Project
{

    /// <summary>
    /// A request body to create a new project. Projects can contain conferences.
    /// </summary>
    public class CreateProjectRequest
    {
        /// <summary>
        /// The id of the organization that is hosting this project. Every project can only be hosted by one major organization.
        /// </summary>
        [Required]
        public string OrganisationId { get; set; }

        /// <summary>
        /// The name of the project. For example: Model United Nations Schleswig-Holstein.
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string Name { get; set; }

        /// <summary>
        /// The short name of the project for example: MUN-SH
        /// </summary>
        [Required]
        [MaxLength(16)]
        public string Abbreviation { get; set; }
    }
}
