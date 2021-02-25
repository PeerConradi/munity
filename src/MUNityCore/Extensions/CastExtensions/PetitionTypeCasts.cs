using MUNity.Schema.Simulation;
using MUNityCore.Models.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Extensions.CastExtensions
{
    public static class PetitionTypeCasts
    {
        public static PetitionTypeDto ToPetitionTypeDto(this PetitionType petitionType)
        {
            var mdl = new PetitionTypeDto()
            {
                Category = petitionType.Category,
                Description = petitionType.Description,
                Name = petitionType.Name,
                PetitionTypeId = petitionType.PetitionTypeId,
                Reference = petitionType.Reference,
                Ruling = petitionType.Ruling
            };
            return mdl;
        }
    }
}
