using MUNity.Models.Simulation;
using MUNityBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
{
    public class CreatePetitionRequest : SimulationRequest, IPetition
    {
        public string PetitionId { get; set; }
        public int PetitionTypeId { get; set; }
        public EPetitionStates Status { get; set; }
        public string Text { get; set; }
        public DateTime PetitionDate { get; set; }
        public int PetitionUserId { get; set; }
        public int TargetAgendaItemId { get; set; }
    }
}
