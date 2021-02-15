using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Conference
{
    /// <summary>
    /// A structure given by the MUNity API when requesting small information of a committee.
    /// </summary>
    public class CommitteeSmallInfo
    {
        /// <summary>
        /// The id of the committee
        /// </summary>
        public string CommitteeId { get; set; }

        /// <summary>
        /// The default display name of the committee. Like General Assembly, Generalversammlung
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The long version of the name of the committee, like: United Nations General Assembly, MUN-SH 2021 Generalversammlung.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// A short name for the committee like: GA, GV
        /// </summary>
        public string CommitteeShort { get; set; }
    }
}
