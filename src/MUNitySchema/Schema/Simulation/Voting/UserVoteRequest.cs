using MUNity.Base;
using MUNity.Schema.Simulation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation.Voting
{
    public class UserVoteRequest : SimulationRequest
    {
        public string VotingId { get; set; }
    
        public EVoteStates Choice { get; set; }
    }
}
