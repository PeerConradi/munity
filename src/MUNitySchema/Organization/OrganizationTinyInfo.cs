using System;
using System.Collections.Generic;
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
}
