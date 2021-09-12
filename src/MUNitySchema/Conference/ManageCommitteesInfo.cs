using MUNity.Schema.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Conference
{
    public class ManageCommitteesInfo : IConferenceBreadcrumb
    {
        public string OrganizationId { get; set; }

        public string OrganizationShort { get; set; }

        public string ProjectId { get; set; }

        public string ProjectShort { get; set; }
        public string OrganizationName { get; set; }
        public string ProjectName { get; set; }
        public string ConferenceId { get; set; }
        public string ConferenceName { get; set; }
        public string ConferenceShort { get; set; }

        public List<ManageCommitteeInfo> Committees { get; set; }

    }

    public class ManageCommitteeInfo
    {
        public string CommitteeId { get; set; }

        public string CommitteeName { get; set; }

        public string CommitteeShort { get; set; }

        public string ParentCommitteeId { get; set; }

        public string ParentCommitteeName { get; set; }

        public int SeatCount { get; set; }
    }


}
