using MUNity.Database.Models.LoS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Models.VirtualCommittee
{
    public class VirtualCommittee
    {
        public string VirtualCommitteeId { get; set; }

        public string Name { get; set; }

        public string AdminPassword { get; set; }

        public string JoinPassword { get; set; }

        public ICollection<VirtualCommitteeSlot> VirtualCommitteeSlots { get; set; }

        public ListOfSpeakers ListOfSpeakers { get; set; }
    }

    public class VirtualCommitteeSlot
    {
        public string VirtualCommitteeSlotId { get; set; }

        public string RoleName { get; set; }

        public string DisplayUserName { get; set; }

        public string UserToken { get; set; }

        public VirtualCommittee VirtualCommittee { get; set; }

        public VirtualCommitteeSlot()
        {
            this.VirtualCommitteeSlotId = Guid.NewGuid().ToString();
            this.UserToken = Util.IdGenerator.RandomString(32);
        }
    }

}
