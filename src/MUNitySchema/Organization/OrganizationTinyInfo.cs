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

    public class OrganizationDashboardInfo
    {
        public string OrganizationId { get; set; }

        public string Name { get; set; }

        public string Short { get; set; }

        public List<OrganizationDashboardProjectInfo> Projects { get; set; }

        public List<OrganizationMemberInfo> Memebrs { get; set; }
    }

    public class OrganizationDashboardProjectInfo
    {
        public string ProjectId { get; set; }

        public string Name { get; set; }

        public int ConferenceCount { get; set; }
    }

    public class OrganizationMemberInfo
    {
        public string MemberUserName { get; set; }

        public string Forename { get; set; }

        public string LastName { get; set; }

        public string RoleName { get; set; }
    }
}
