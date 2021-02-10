using MUNity.Schema.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Simulation
{
    public class PetitionTypeSimulation
    {
        public long PetitionTypeSimulationId { get; set; }

        public Simulation Simulation { get; set; }

        public PetitionType PetitionType { get; set; }

        public int OrderIndex { get; set; }

        public bool AllowChairs { get; set; } = false;

        public bool AllowDelegates { get; set; } = true;

        public bool AllowNgo { get; set; } = false;

        public bool AllowSpectator { get; set; } = false;

        internal PetitionTypeSimulationDto ToDto()
        {
            var dto = new PetitionTypeSimulationDto()
            {
                AllowChairs = AllowChairs,
                AllowNgo = AllowNgo,
                AllowDelegates = AllowDelegates,
                AllowSpectator = AllowSpectator,
                Category = PetitionType.Category,
                Description = PetitionType.Description,
                Name = PetitionType.Name,
                PetitionTypeId = PetitionType.PetitionTypeId,
                Reference = PetitionType.Reference,
                Ruling = PetitionType.Ruling
            };
            return dto;
        }
    }
}
