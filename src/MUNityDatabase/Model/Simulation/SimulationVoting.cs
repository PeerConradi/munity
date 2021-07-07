using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Simulation
{
    public class SimulationVoting
    {
        public string SimulationVotingId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Simulation Simulation { get; set; }

        public ICollection<SimulationVotingSlot> VoteSlots { get; set; }

        public bool IsActive { get; set; }

        public bool AllowAbstention { get; set; }

        public SimulationVoting()
        {
            SimulationVotingId = Guid.NewGuid().ToString();
        }
    }

}
