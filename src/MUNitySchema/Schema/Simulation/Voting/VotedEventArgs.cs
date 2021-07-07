using MUNityBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation
{

    /// <summary>
    /// Event Arguments for a given vote for the simulation socket.
    /// </summary>
    public class VotedEventArgs : EventArgs
    {
        /// <summary>
        /// The id of the voting
        /// </summary>
        public string VoteId { get; set; }

        /// <summary>
        /// the simulationUserId of the user that has voted.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The index of the choice that this user has picked.
        /// </summary>
        public EVoteStates Choice { get; set; }

        /// <summary>
        /// Creates a new Instance of this voting with all given arguments
        /// </summary>
        /// <param name="voteId"></param>
        /// <param name="userId"></param>
        /// <param name="choice"></param>
        public VotedEventArgs(string voteId, int userId, EVoteStates choice)
        {
            this.VoteId = voteId;
            this.UserId = userId;
            this.Choice = choice;
        }

        /// <summary>
        /// Creates a new instance of voting.
        /// </summary>
        public VotedEventArgs()
        {

        }
    }
}
