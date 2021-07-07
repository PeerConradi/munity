using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Dtos.Simulations
{
    public class AgendaItemInfo
    {
        public int AgendaItemId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int PetitionCount { get; set; }
    }
}
