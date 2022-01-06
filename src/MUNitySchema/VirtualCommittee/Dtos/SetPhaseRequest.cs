using MUNity.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.VirtualCommittee.Dtos
{
    public class SetPhaseRequest : SimulationRequest
    {
        public GamePhases SimulationPhase { get; set; }
    }
}
