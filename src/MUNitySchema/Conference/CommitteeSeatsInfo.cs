using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Conference
{
    public class CommitteeSeatsInfo
    {
        public string CommitteeId { get; set; }

        public string CommitteeName { get; set; }

        public string CommitteeShort { get; set; }

        public string CommitteeArticle { get; set; }

        public List<CommitteeSeatInfo> Seats { get; set; }

        public List<CountryInfo> Countries { get; set; }

        public List<DelegationInfo> Delegations { get; set; }
    }

    public class CommitteeSeatInfo
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public int? CountryId { get; set; }

        public string CountryName { get; set; }

        public string DelegationId { get; set; }

        public string DelegationName { get; set; }

        public string Subtypes { get; set; }

        public string Secret { get; set; }

        public List<CommitteeParticipation> Participants { get; set; }
    }

    public class CommitteeParticipation
    {
        public int ParticipationId { get; set; }

        public string Username { get; set; }

        public string DisplayName { get; set; }
    }

    public class CountryInfo
    {
        public int CountryId { get; set; }

        public string Name { get; set; }
    }

    public class DelegationInfo
    {
        public string DelegationId { get; set; }

        public string DelegationName { get; set; }
    }
}
