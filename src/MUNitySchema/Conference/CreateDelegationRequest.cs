using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Conference
{
    public class CreateDelegationRequest
    {
        public string ConferenceId { get; set; }

        public string DelegationName { get; set; }

        public string DelegationFullName { get; set; }

        public string DelegationShort { get; set; }
    }

    public class CreateDelegationResponse : AbstractResponse
    {
        public string NewDelegationId { get; set; }
    }
}
