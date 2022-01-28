using MUNity.Base;
using MUNity.Models.Simulation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.VirtualCommittees.Dtos
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
