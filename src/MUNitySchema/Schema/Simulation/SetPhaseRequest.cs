using MUNity.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
{
    public class SetPhaseRequest : SimulationRequest
    {
        public GamePhases SimulationPhase { get; set; }
    }
}
