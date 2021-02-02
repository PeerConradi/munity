using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MUNity.Schema.Organization
{
    /// <summary>
    /// The request body to create an organization. Every project needs a hosting organization and
    /// you need a project to create a conference.
    /// </summary>
    public class CreateOrganizationRequest
    {
        /// <summary>
        /// The Name of the organization for example: Deutsche Model United Nations e.V.
        /// The max length of an organization name is 300 characters.
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string OrganizationName { get; set; }

        /// <summary>
        /// The short name of the organization for example DMUN e.V.
        /// Max length of the abbreviation (short name) is 16 characters.
        /// </summary>
        [Required]
        [MaxLength(16)]
        public string Abbreviation { get; set; }
    }
}
