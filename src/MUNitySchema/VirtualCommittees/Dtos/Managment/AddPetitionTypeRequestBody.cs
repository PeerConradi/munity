using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.VirtualCommittees.Dtos.Managment
{
    public class AddPetitionTypeRequestBody : SimulationRequest
    {
        public int PetitionTypeId { get; set; }

        public int OrderIndex { get; set; } = -1;

        public bool AllowChairs { get; set; } = false;

        public bool AllowDelegates { get; set; } = true;

        public bool AllowNgo { get; set; } = false;

        public bool AllowSpectator { get; set; } = false;
    }
}
