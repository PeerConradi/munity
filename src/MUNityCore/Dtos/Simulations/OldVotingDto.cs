using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Dtos.Simulations
{
    public class OldVotingDto
    {
        public string Name { get; set; }

        public int ProCount { get; set; }

        public int ConCount { get; set; }

        public int AbstentionCount { get; set; }

        public int NotVotedCount { get; set; }
    }
}
