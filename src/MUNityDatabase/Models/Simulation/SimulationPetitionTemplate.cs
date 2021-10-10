using MUNityBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Simulation
{
    public class SimulationPetitionTemplate
    {
        public int SimulationPetitionTemplateId { get; set; }

        public string TemplateName { get; set; }

        public ICollection<PetitionTemplateEntry> Entries { get; set; }


    }

    public class PetitionTemplateEntry
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Reference { get; set; }

        public PetitionRulings Ruling { get; set; }

        public string Category { get; set; }

        public bool AllowChairs { get; set; }

        public bool AllowDelegates { get; set; }

        public bool AllowNgo { get; set; }

        public bool AllowSpectator { get; set; }
    }
}
