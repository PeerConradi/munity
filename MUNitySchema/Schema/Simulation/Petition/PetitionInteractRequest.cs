using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
{
    public class PetitionInteractRequest : SimulationRequest
    {
        public int AgendaItemId { get; set; }
        public string PetitionId { get; set; }
    }
}
