using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.VirtualCommittees.Dtos
{
    /// <summary>
    /// Schema for created votes.
    /// This SChema can be sent to everyone and the server or client
    /// implementation needs to handle the rights to vote.
    /// 
    /// This way its possible for everyone to see that a voting is ongoing 
    /// and who as already voted and who has voted for what.
    /// 
    /// There is no secret voting in this implementation!
    /// </summary>
    public class CreatedVoteModel
    {
        /// <summary>
        /// The Id of the new created Voting.
        /// </summary>
        public string CreatedVoteModelId { get; set; }

        /// <summary>
        /// The DIsplay Text of the voting.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Is voting abstent allowed.
        /// </summary>
        public bool AllowAbstention { get; set; }

        /// <summary>
        /// List of the userIds that are allowed to vote.
        /// </summary>
        public List<int> AllowedUsers { get; set; }
    }
}
