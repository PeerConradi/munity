using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Conference
{
    public class ConferenceLabel
    {
        public long ConferenceLabelId { get; set; }

        public MUNity.Database.Models.Conference.Conference Conference { get; set; }

        public string ConferenceLabelName { get; set; }

        public string ConferenceLabelValue { get; set; }

        public static class KnownLabels
        {
            public const string COMMITTEE_LABEL = "Committee";

            public const string DELEGATION_LABEL = "Delegation";

            public const string ROLE_LABEL = "Role";

        }
    }


}
