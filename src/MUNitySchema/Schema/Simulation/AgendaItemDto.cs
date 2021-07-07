using MUNity.Models.Simulation;
using MUNityBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
{
    public class AgendaItemDto
    {
        public int AgendaItemId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public EAgendaItemStatuses Status { get; set; }

        public List<PetitionDto> Petitions { get; set; }
    }
}
