using MUNity.Schema.General;
using System;

namespace MUNity.Schema.VirtualCommittee.ViewModels
{
    public class VirtualCommitteeSlotViewModel : AbstractViewModel
    {
        public string VirtualCommitteeSlotId { get; set; }

        public string RoleName { get; set; }

        public string DisplayUserName { get; set; }

        public string UserToken { get; set; }

        public VirtualCommitteeSlotViewModel()
        {
            this.VirtualCommitteeSlotId = Guid.NewGuid().ToString();
            this.UserToken = Util.IdGenerator.RandomString(32);
        }
    }

    
}
