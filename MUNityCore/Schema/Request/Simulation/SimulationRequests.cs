using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Schema.Request.Simulation
{
    public class SimulationRequests
    {
        public struct CreateSimulation
        {
            [Required]
            public string Name { get; set; }

            public string Password { get; set; }

            [Required]
            public string AdminPassword { get; set; }

            [Required]
            public string UserDisplayName { get; set; }
        }
    }
}
