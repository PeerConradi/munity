using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.EntityFramework.Models
{
    public class CommitteeLeader
    {
        public int CommitteeLeadId { get; set; }

        public Committee Committee { get; set; }

        public User User { get; set; }

        public string RoleName { get; set; }
    }
}
