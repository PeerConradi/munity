using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
{
    public class SetResolutionOnlineAmendmentState : SimulationRequest
    {
        public string ResolutionId { get; set; }

        public bool State { get; set; }
    }
}
