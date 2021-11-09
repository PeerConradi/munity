﻿using MUNity.Database.Models.Simulation;
using MUNity.Schema.Simulation;

namespace MUNity.Services.Extensions.CastExtensions;

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
