using System;
using System.Collections.Generic;
using System.Text;
using MUNity.Schema.General;

namespace MUNity.Schema.Conference
{
    public class ConferenceRolesInfo : IConferenceBreadcrumb
    {
        public string OrganizationId { get; set; }
        public string OrganizationShort { get; set; }
        public string OrganizationName { get; set; }
        public string ProjectId { get; set; }
        public string ProjectShort { get; set; }
        public string ProjectName { get; set; }
        public string ConferenceId { get; set; }
        public string ConferenceName { get; set; }
        public string ConferenceShort { get; set; }

        public List<ManageDelegationRoleInfo> Roles { get; set; }

        public List<DelegationInfo> Delegations { get; set; }

        public List<CountryInfo> Countries { get; set; }
    }
}
