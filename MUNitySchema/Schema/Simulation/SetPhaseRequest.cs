using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
{
    public class SetPhaseRequest : SimulationRequest
    {
        public SimulationEnums.GamePhases SimulationPhase { get; set; }
    }
}
