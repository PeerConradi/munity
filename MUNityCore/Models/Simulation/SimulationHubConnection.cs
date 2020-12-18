using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Simulation
{
    public class SimulationHubConnection
    {
        public int SimulationHubConnectionId { get; set; }

        public SimulationUser User { get; set; }

        public string ConnectionId { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
