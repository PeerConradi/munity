using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.Conference;

namespace MUNityAngular.Models.Conference
{

    /// <summary>
    /// The committees are listed inside each Conference. Every conference
    /// needs to create its own list of committees, they should not be reused inside of other
    /// Conferences.
    /// </summary>
    public class Committee
    {
        public string CommitteeId { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string Abbreviation { get; set; }

        public string Article { get; set; }

        public Committee ResolutlyCommittee { get; set; }

        public Conference Conference { get; set; }

        public List<CommitteeTopic> Topics { get; set; }

        public Committee()
        {
            CommitteeId = Guid.NewGuid().ToString();
        }
    }
}
