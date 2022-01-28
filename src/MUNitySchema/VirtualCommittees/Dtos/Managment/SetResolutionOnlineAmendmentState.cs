using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.VirtualCommittees.Dtos
{
    public class SetResolutionOnlineAmendmentState : SimulationRequest
    {
        public string ResolutionId { get; set; }

        public bool State { get; set; }
    }
}
