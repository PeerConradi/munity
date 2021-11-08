using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Conference
{
    public class UserApplicationInfo
    {
        public int ApplicationId { get; set; }

        public string ConferenceId { get; set; }

        public string ConferenceName { get; set; }

        public string ConferenceFullName { get; set; }

        public string ConferenceShort { get; set; }

        public List<UserApplicationDelegation> Delegations { get; set; }
    }

    public class UserApplicationDelegation
    {
        public string DelegationId { get; set; }

        public string DelegationName { get; set; }
    }
}
