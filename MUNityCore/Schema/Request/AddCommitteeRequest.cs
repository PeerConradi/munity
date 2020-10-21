using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Schema.Request
{
    public class AddCommitteeRequest
    {
        public string ConferenceId { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string Abbreviation { get; set; }

        public string Article { get; set; }

        public string ResolutlyCommittee { get; set; }
    }
}
