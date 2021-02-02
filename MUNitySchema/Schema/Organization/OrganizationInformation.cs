using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Organization
{
    /// <summary>
    /// The base model you will get by the MUNity API when requestiong an organization.
    /// </summary>
    public class OrganizationInformation
    {
        /// <summary>
        /// The id of the organization.
        /// </summary>
        public string OrganizationId { get; set; }

        /// <summary>
        /// The name of the organization for example Deutsche Model United Nations e.V.
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// The short name of the organization for example DMUN e.V.
        /// </summary>
        public string OrganizationShort { get; set; }
    }
}
