using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.VirtualCommittees.Dtos
{
    public class SimulationStatusDto
    {
        public int SimulationStatusId { get; set; }

        public string StatusText { get; set; }

        public DateTime StatusTime { get; set; }
    }
}
