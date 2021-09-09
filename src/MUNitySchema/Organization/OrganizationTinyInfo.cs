using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MUNity.Schema.Organization
{
    public class OrganizationTinyInfo
    {
        public string OrganizationId { get; set; }

        public string Short { get; set; }

        public string Name { get; set; }
    }

    public class OrganizationWithConferenceInfo : OrganizationTinyInfo
    {
        public int ProjectCount { get; set; }

        public int ConferenceCount { get; set; }
    }

    public class CreateOrganizationRequest
    {
        [Required]
        [MinLength(2)]
        public string ShortName { get; set; }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }
    }

    public class CreateOrganizationResponse
    {
        public enum CreateOrgaStatusCodes
        {
            Success,
            NameTaken,
            ShortTaken,
            Error
        }

        public CreateOrgaStatusCodes Status { get; set; }

        public string OrganizationId { get; set; }
    }
}
