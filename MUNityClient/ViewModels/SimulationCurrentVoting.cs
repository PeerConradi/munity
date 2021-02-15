using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityClient.ViewModels
{
    public class SimulationCurrentVoting : MUNity.Schema.Simulation.CreatedVoteModel
    {
        /// <summary>
        /// Dictionary of given votes. The Key is the SimulationUserId and the Value is the vote
        /// 0 = pro, 1 = contra, 2 = abstention
        /// </summary>
        public Dictionary<int, int> Choices { get; private set; }

        public int TotalVotes => Choices.Count;

        public int ValidVotes => Choices.Count(n => n.Value == 0 || n.Value == 1);

        public int ProVotes => Choices.Count(n => n.Value == 0);

        public int ContraVotes => Choices.Count(n => n.Value == 1);

        public int AbstentionVotes => Choices.Count(n => n.Value == 2);

        public int PercentageMissing => (this.AllowedUsers.Count - Choices.Count) / this.AllowedUsers.Count * 100;

        public int PercentagePro => (ProVotes + ContraVotes) > 0 ? (ProVotes * 100 / (ProVotes + ContraVotes)) : 0;

        public int PercentageContra => (ProVotes + ContraVotes) > 0 ? (ContraVotes * 100 / (ProVotes + ContraVotes)) : 0;

        public int PercentageAbstention => Choices.Count > 0 ? (AbstentionVotes * 100 / Choices.Count) : 0;

        public void Vote(MUNity.Schema.Simulation.VotedEventArgs args)
        {
            if (args.VoteId != this.CreatedVoteModelId) return;
            if (this.Choices.ContainsKey(args.UserId)) return;
            this.Choices.Add(args.UserId, args.Choice);
        }

        public SimulationCurrentVoting(MUNity.Schema.Simulation.CreatedVoteModel created)
        {
            this.Choices = new Dictionary<int, int>();
            this.AllowAbstention = created.AllowAbstention;
            this.AllowedUsers = created.AllowedUsers;
            this.CreatedVoteModelId = created.CreatedVoteModelId;
            this.Text = created.Text;
        }
    }
}
