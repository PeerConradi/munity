using MUNity.Base;
using System;
using System.Collections.Generic;
using System.Text;
using static MUNity.VirtualCommittee.Dtos.PetitionTypeDto;

namespace MUNity.VirtualCommittee.Dtos.Managment
{
    public class PossiblePetitionsResponse
    {
        public long PetitionTypeSimulationId { get; set; }

        public string TypeName { get; set; }

        public string TypeDescription { get; set; }

        public string TypeReference { get; set; }

        public PetitionRulings TypeRuling { get; set; }

        public string TypeCategory { get; set; }

        public int OrderIndex { get; set; }

        public bool AllowChairs { get; set; }

        public bool AllowDelegates { get; set; }

        public bool AllowNgo { get; set; }

        public bool AllowSpectator { get; set; }
    }
}
