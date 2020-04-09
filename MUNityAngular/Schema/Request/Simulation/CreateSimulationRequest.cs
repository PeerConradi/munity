using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Schema.Request.Simulation
{
    public class CreateSimulationRequest
    {
        public string LobbyName { get; set; }

        public string CreatorName { get; set; }

        public string Password { get; set; }

        public bool Free4All { get; set; }

        public List<string> DelegationIds { get; set; }
    }
}
