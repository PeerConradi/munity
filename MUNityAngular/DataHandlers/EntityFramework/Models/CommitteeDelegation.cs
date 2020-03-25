using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.EntityFramework.Models
{
    public class CommitteeDelegation
    {
        public int CommitteeDelegationId { get; set; }

        public Committee Committee { get; set; }

        public Delegation Delegation { get; set; }

        public int MinCount { get; set; }

        public int MaxCount { get; set; }
    }
}
