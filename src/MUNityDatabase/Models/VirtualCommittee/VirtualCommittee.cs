using MUNity.Base;
using MUNity.Database.Models.LoS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Models.VirtualCommittees
{
    public class VirtualCommitteeGroup
    {
        public string VirtualCommitteeGroupId { get; set; }

        public string GroupName { get; set; }

        public ICollection<VirtualCommittee> VirtualCommittees { get; set; }

        public VirtualCommitteeGroup()
        {
            VirtualCommitteeGroupId = Guid.NewGuid().ToString();
            VirtualCommittees = new List<VirtualCommittee>();
        }
    }

    public class VirtualCommittee
    {
        public string VirtualCommitteeId { get; set; }

        public string Name { get; set; }

        public string AdminPassword { get; set; }

        public string JoinPassword { get; set; }

        public VirtualCommitteeGroup Group { get; set; }

        public ICollection<VirtualCommitteeSlot> VirtualCommitteeSlots { get; set; }

        public ListOfSpeakers ListOfSpeakers { get; set; }

        public VirtualCommittee()
        {
            VirtualCommitteeId = Guid.NewGuid().ToString();
        }
    }

    public class VirtualCommitteeSlot
    {
        

        public string VirtualCommitteeSlotId { get; set; }

        public string Iso { get; set; }

        public string RoleName { get; set; }

        public SlotTypes SlotType { get; set; } = SlotTypes.Visitor;

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
