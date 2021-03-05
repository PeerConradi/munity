using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityClient.ViewModels
{
    public class SimulationCurrentVoting : MUNity.Schema.Simulation.Voting.SimulationVotingDto
    { 

        public int TotalVotes => Slots.Count;

        public int ValidVotes => Slots.Count(n => n.Choice == MUNity.Schema.Simulation.EVoteStates.Con || 
        n.Choice == MUNity.Schema.Simulation.EVoteStates.Pro);

        public int ProVotes => Slots.Count(n => n.Choice == MUNity.Schema.Simulation.EVoteStates.Pro);

        public int ContraVotes => Slots.Count(n => n.Choice == MUNity.Schema.Simulation.EVoteStates.Con);

        public int AbstentionVotes => Slots.Count(n => n.Choice == MUNity.Schema.Simulation.EVoteStates.Abstention);

        public int NotVoted => Slots.Count(n => n.Choice == MUNity.Schema.Simulation.EVoteStates.NotVoted);

        public int PercentageMissing => (Slots.Count > 0) ? NotVoted * 100 / this.Slots.Count : 0;

        public int PercentagePro => (ProVotes + ContraVotes) > 0 ? (ProVotes * 100 / (ProVotes + ContraVotes)) : 0;

        public int PercentageContra => (ProVotes + ContraVotes) > 0 ? (ContraVotes * 100 / (ProVotes + ContraVotes)) : 0;

        public int PercentageAbstention => Slots.Count > 0 ? (AbstentionVotes * 100 / Slots.Count) : 0;

        public SimulationCurrentVoting(MUNity.Schema.Simulation.Voting.SimulationVotingDto created)
        {
            this.AllowAbstention = created.AllowAbstention;
            this.Slots = created.Slots;
            this.Name = created.Name;
        }
    }
}
