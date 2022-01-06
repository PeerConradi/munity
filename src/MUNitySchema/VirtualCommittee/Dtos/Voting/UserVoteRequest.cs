using MUNity.Base;
using MUNity.VirtualCommittee.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.VirtualCommittee.Dtos.Voting
{
    public class UserVoteRequest : SimulationRequest
    {
        public string VotingId { get; set; }
    
        public EVoteStates Choice { get; set; }
    }
}
