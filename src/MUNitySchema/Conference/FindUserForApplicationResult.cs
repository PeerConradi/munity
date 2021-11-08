using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Conference
{
    public class FindUserForApplicationResult
    {
        public enum ResultStatuses
        {
            InvalidInput,
            CanBeAdded,
            AlreadyParticipating,
            AlreadyApplying,
            NoUserFound
        }

        public ResultStatuses Status { get; set; }

        public string UserName { get; set; }

        public string ForeName { get; set; }

        public string LastName { get; set; }
    }
}
