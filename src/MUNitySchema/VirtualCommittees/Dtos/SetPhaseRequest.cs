using MUNity.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.VirtualCommittees.Dtos
{
    public class SetPhaseRequest : SimulationRequest
    {
        public GamePhases SimulationPhase { get; set; }
    }
}
