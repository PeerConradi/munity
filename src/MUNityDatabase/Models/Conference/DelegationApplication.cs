using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNity.Database.Models.Website;
using MUNityBase;

namespace MUNity.Database.Models.Conference
{
    public class DelegationApplication
    {
        public int DelegationApplicationId { get; set; }

        public ICollection<DelegationApplicationPickedDelegation> DelegationWishes { get; set; }

        public ICollection<DelegationApplicationUserEntry> Users { get; set; }

        public DateTime ApplyDate { get; set; }

       public ICollection<ConferenceDelegationApplicationFieldInput> FormulaInputs { get; set; }

        /// <summary>
        /// Are Others able to see this application and add themselfs to it.
        /// </summary>
        public bool OpenToPublic { get; set; }

        public string Expose { get; set; }

        public ApplicationStatuses Status { get; set; }

        /// <summary>
        /// Guests are people like teachers etc.
        /// when they are included a new empty Delegate Role needs to be created
        /// for each guest.
        /// </summary>
        public int? GuestsCount { get; set; }

        /// <summary>
        /// It is possible to get the Conference by using the Delegation Wishes,
        /// but once an Applicaiton has no wishes on it, it can't be found anymore
        /// Thats why there is also a Conference Tag here.
        /// </summary>
        public Conference Conference { get; set; }
    }
}
