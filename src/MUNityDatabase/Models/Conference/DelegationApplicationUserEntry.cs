using MUNity.Base;
using MUNity.Database.Models.User;
using System.Collections.Generic;

namespace MUNity.Database.Models.Conference
{
    public class DelegationApplicationUserEntry
    {
        public int DelegationApplicationUserEntryId { get; set; }

        public DelegationApplication Application { get; set; }

        public MunityUser User { get; set; }

        public DelegationApplicationUserEntryStatuses Status { get; set; }

        public bool CanWrite { get; set; }

        public string Message { get; set; }

        public ICollection<ConferenceDelegationApplicationUserFieldInput> CustomUserInputs { get; set; }
    }
}
