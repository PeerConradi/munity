using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MUNity.Schema.Simulation
{
    public class SetSimulationStatusDto : SimulationRequest
    {
        [Required]
        public string StatusText { get; set; }
    }
}
