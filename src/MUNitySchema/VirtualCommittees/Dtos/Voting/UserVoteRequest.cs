using MUNity.Base;
using MUNity.VirtualCommittees.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.VirtualCommittees.Dtos.Voting
{
    public class UserVoteRequest : SimulationRequest
    {
        public string VotingId { get; set; }
    
        public EVoteStates Choice { get; set; }
    }
}
