using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Schema.Response
{
    public class OrganizationInformation
    {
        public string OrganizationId { get; set; }

        public string OrganizationName { get; set; }

        public string OrganizationShort { get; set; }

        public OrganizationInformation(Models.Organization.Organization organization)
        {
            this.OrganizationId = organization.OrganizationId;
            this.OrganizationName = organization.OrganizationName;
            this.OrganizationShort = organization.OrganizationShort;
        }

        public static implicit operator OrganizationInformation (Models.Organization.Organization organization)
        {
            return new OrganizationInformation(organization);
        }
    }
}
