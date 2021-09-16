using MUNity.Schema.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Conference
{
    public class ManageDelegationsInfo : IConferenceBreadcrumb
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

        public List<ManageDelegationInfo> Delegations { get; set; }
    }

    public class ManageDelegationInfo
    {
        public string DelegationId { get; set; }

        public string DelegationName { get; set; }

        public string DelegationShort { get; set; }

        public List<ManageDelegationRoleInfo> Roles { get; set; }
    }

    public class ManageDelegationRoleInfo
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string RoleCommitteeId { get; set; }

        public string RoleCommitteeName { get; set; }

        public bool HasParicipant { get; set; }

        public string Subtype { get; set; }

        public MUNityBase.EApplicationStates ApplicationState { get; set; }
    }
}
