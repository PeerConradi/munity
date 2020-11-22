using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Schema.Response.Conference
{
    public class CommitteeSmallInfo
    {
        public string CommitteeId { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string CommitteeShort { get; set; }

        public CommitteeSmallInfo(Models.Conference.Committee committee)
        {
            this.CommitteeId = committee.CommitteeId;
            this.Name = committee.Name;
            this.FullName = committee.FullName;
            this.CommitteeShort = committee.CommitteeShort;
        }

        public static implicit operator CommitteeSmallInfo (Models.Conference.Committee committee)
        {
            return new CommitteeSmallInfo(committee);
        }
    }
}
