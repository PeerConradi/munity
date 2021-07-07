using MUNityBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUNity.Schema.Simulation.Voting
{
    public class SimulationVotingDto
    {
        public string VotingId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool AllowAbstention { get; set; }

        public List<SimulationVoteSlotDto> Slots { get; set; } = new List<SimulationVoteSlotDto>();

        public int TotalValidVotes => Slots.Count(n => n.Choice == EVoteStates.Pro || n.Choice == EVoteStates.Con);

        public int TotalGivenVotes => Slots.Count(n => n.Choice != EVoteStates.NotVoted);

        public int ProVotes => Slots.Count(n => n.Choice == EVoteStates.Pro);

        public int ConVotes => Slots.Count(n => n.Choice == EVoteStates.Con);

        public int AbstentionVotes => Slots.Count(n => n.Choice == EVoteStates.Abstention);

        public double ProPercentageOverall
        {
            get
            {
                if (Slots == null || Slots.Count == 0)
                    return 0;
                return Math.Round(ProVotes / (double)Slots.Count * 100, 2);
            }
        }

        public double ConPercentageOverall
        {
            get
            {
                if (Slots == null || Slots.Count == 0)
                    return 0;
                return Math.Round(ConVotes / (double)Slots.Count * 100, 2);
            }
        }

        public double AbstentionPercentageOverall
        {
            get
            {
                if (Slots == null || Slots.Count == 0)
                    return 0;
                return Math.Round(AbstentionVotes / (double)Slots.Count * 100, 2);
            }
        }

        public double ProPercentage
        {
            get
            {
                if (this.TotalValidVotes == 0) return 0;
                return Math.Round(this.ProVotes / (double)this.TotalValidVotes * 100, 2);
            }
        }

        public double ConPercentage
        {
            get
            {
                if (this.TotalValidVotes == 0) return 0;
                return Math.Round(this.ConVotes / (double)this.TotalValidVotes * 100, 2);
            }
        }
    }

    public class SimulationVoteSlotDto
    {
        public int SimulationVoteSlotId { get; set; }

        public int SimulationUserId { get; set; }

        public string DisplayName { get; set; }

        public string RoleName { get; set; }

        public EVoteStates Choice { get; set; }

        public DateTime? VoteTime { get; set; }
    }
}
