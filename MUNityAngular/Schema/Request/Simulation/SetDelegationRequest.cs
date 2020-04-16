using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Schema.Request.Simulation
{
    public class SetDelegationRequest
    {
        public string SimulationId { get; set; }

        public string Token { get; set; }

        public string DelegationId { get; set; }
    }
}
