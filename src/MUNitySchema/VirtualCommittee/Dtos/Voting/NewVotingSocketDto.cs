using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.VirtualCommittee.Dtos.Voting
{
    public class NewVotingSocketDto
    {
        public int SimulationId { get; set; }

        public string DisplayName { get; set; }

        public bool AllowAbstention { get; set; } = false;

        public bool AllowNgoVote { get; set; } = false;

        public int PresentsId { get; set; } = -1;

        public List<int> VoteExceptions { get; set; } = new List<int>();
    }
}
