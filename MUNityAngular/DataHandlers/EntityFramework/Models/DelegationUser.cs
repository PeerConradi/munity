using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.EntityFramework.Models
{
    public class DelegationUser
    {
        public int DelegationUserId {get; set; }

        public Delegation Delegation { get; set; }

        public Committee Committee { get; set; }

        public User User { get; set; }

        public bool IsLeader { get; set; }

        public DateTime JoinDate { get; set; }

    }
}
