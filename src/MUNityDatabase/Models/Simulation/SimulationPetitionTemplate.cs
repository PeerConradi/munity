using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Simulation;

public class SimulationPetitionTemplate
{
    public int SimulationPetitionTemplateId { get; set; }

    public string TemplateName { get; set; }

    public ICollection<PetitionTemplateEntry> Entries { get; set; }


}
