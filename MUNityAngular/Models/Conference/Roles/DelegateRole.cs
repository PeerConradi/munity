using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference.Roles
{
    public class DelegateRole : AbstractRole
    {
        public State DelegateState { get; set; }

        public bool IsDelegationLeader { get; set; }

        public string Title { get; set; }

        public Delegation Delegation { get; set; }

        public Committee Committee { get; set; }
    }
}
