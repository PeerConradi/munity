using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation.Voting
{
    public class SimulationVotingDto
    {
        public string VotingId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool AllowAbstention { get; set; }

        public List<SimulationVoteSlotDto> Slots { get; set; }
    }

    public class SimulationVoteSlotDto
    {
        public int SimulationVoteSlotId { get; set; }

        public int SimulationUserId { get; set; }

        public EVoteStates Choice { get; set; }

        public DateTime? VoteTime { get; set; }
    }
}
