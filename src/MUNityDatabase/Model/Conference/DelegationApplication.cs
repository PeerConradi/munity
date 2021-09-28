using MUNity.Database.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNity.Database.Models.Conference.Roles;
using MUNityBase;

namespace MUNity.Database.Models.Conference
{
    public class DelegationApplication
    {
        public int DelegationApplicationId { get; set; }

        public ICollection<DelegationApplicationPickedDelegation> Delegations { get; set; }

        public ICollection<DelegationApplicationUserEntry> Users { get; set; }

        public DateTime ApplyDate { get; set; }

        public string Title { get; set; }

        public string Motivation { get; set; }

        public string Experience { get; set; }

        public string SchoolName { get; set; }

        public int? SchoolId { get; set; }

        /// <summary>
        /// Are Others able to see this application and add themselfs to it.
        /// </summary>
        public bool OpenToPublic { get; set; }

        public ApplicationStatuses Status { get; set; }

    }

    public class DelegationApplicationUserEntry
    {
        public int DelegationApplicationUserEntryId { get; set; }

        public DelegationApplication Application { get; set; }

        public MunityUser User { get; set; }

        public DelegationApplicationUserEntryStatuses Status { get; set; }
    }

    public class DelegationApplicationPickedDelegation
    {
        public int DelegationApplicationPickedDelegationId { get; set; }

        public DelegationApplication Application { get; set; }

        public short Priority { get; set; }

        public Delegation Delegation { get; set; }
    }
}
