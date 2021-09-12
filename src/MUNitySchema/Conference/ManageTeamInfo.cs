using MUNity.Schema.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Conference
{
    public class ManageTeamInfo : IConferenceBreadcrumb
    {
        public string OrganizationName { get; set; }

        public string OrganizationShort { get; set; }

        public string OrganizationId { get; set; }

        public string ProjectName { get; set; }

        public string ProjectShort { get; set; }

        public string ProjectId { get; set; }

        public string ConferenceName { get; set; }

        public string ConferenceShort { get; set; }

        public string ConferenceId { get; set; }

        public List<TeamRoleGroupInfo> RoleGroups { get; set; }
    }

    public class TeamRoleGroupInfo
    {
        public int TeamRoleGroupId { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string Short { get; set; }

        public List<TeamRoleInfo> Roles { get; set; }
    }

    public class TeamRoleInfo
    {
        public int TeamRoleId { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string Short { get; set; }

        public int? ParentId { get; set; }

        public string ParentName { get; set; }

        public List<RoleParticipant> Participants { get; set; }
    }

    public class RoleParticipant
    {
        public string ParticipantUsername { get; set; }

        public string ParticipantDisplayName { get; set; }
    }
}
