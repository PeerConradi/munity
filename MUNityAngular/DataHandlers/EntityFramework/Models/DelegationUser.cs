using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.EntityFramework.Models
{
    public class DelegationUser
    {
        public CommitteeDelegation CommitteeDelegation { get; set; }

        public User User { get; set; }

        public bool IsLeader { get; set; }

        public DateTime JoinDate { get; set; }

    }
}
