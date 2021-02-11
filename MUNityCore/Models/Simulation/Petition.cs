using MUNitySchema.Models.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Simulation
{
    public class Petition
    {
        public string PetitionId { get; set; }

        public EPetitionStates Status { get; set; }
        public string Text { get; set; }
        public DateTime PetitionDate { get; set; }

        public PetitionType PetitionType { get; set; }

        public SimulationUser SimulationUser { get; set; }

        public AgendaItem AgendaItem { get; set; }

        public Petition()
        {
            PetitionId = Guid.NewGuid().ToString();
        }
    }
}
