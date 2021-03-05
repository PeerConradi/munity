using MUNity.Schema.Simulation;
using System;

namespace MUNityCore.Models.Simulation
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
