using MUNityBase;
using System;

namespace MUNity.Database.Models.Simulation
{
    public class SimulationVotingSlot
    {
        
        public int SimulationVotingSlotId { get; set; }

        public SimulationVoting Voting { get; set; }

        public SimulationUser User { get; set; }

        public EVoteStates Choice { get; set; }

        public DateTime? VoteTime { get; set; }
    }

}
