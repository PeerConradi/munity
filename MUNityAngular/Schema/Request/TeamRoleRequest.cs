using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Schema.Request
{
    public class TeamRoleRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ParentRoleId { get; set; }

        public string MinCount { get; set; }

        public string MaxCount { get; set; }

        public string ConferenceId { get; set; }
    }
}
