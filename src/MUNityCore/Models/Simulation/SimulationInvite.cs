using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Simulation
{
    public class SimulationInvite
    {
        [MaxLength(100)]
        public string SimulationInviteId { get; set; }

        public SimulationUser User { get; set; }

        public DateTime ExpireDate { get; set; }
    }
}
