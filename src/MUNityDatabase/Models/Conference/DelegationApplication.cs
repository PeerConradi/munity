using MUNity.Database.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MUNity.Database.Models.Website;
using MUNity.Database.Models.Conference.Roles;
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

        public ApplicationStatuses Status { get; set; }

    }



    public class DelegationApplicationUserEntry
    {
        public int DelegationApplicationUserEntryId { get; set; }

        public DelegationApplication Application { get; set; }

        public MunityUser User { get; set; }

        public DelegationApplicationUserEntryStatuses Status { get; set; }

        public bool CanWrite { get; set; }

        public string Message { get; set; }
    }

    public class DelegationApplicationPickedDelegation
    {
        public int DelegationApplicationPickedDelegationId { get; set; }

        public DelegationApplication Application { get; set; }

        public byte Priority { get; set; }

        public Delegation Delegation { get; set; }

        public ICollection<DelegationApplicationPickedDelegationUserWish> UserWishes { get; set; }

        public string Comment { get; set; }
    }

    public class DelegationApplicationPickedDelegationUserWish
    {
        public long DelegationApplicationPickedDelegationUserWishId { get; set; }

        public DelegationApplicationPickedDelegation DelegationApplicationPickedDelegation { get; set; }


        public DelegationApplicationUserEntry UserEntry { get; set; }

        public ConferenceDelegateRole Role { get; set; }

        public byte Prio { get; set; }

        public string Comment { get; set; }
    }
}
