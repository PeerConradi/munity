using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityClient.Services.SocketHandlers
{
    public class VotedEventArgs : EventArgs
    {
        public string VoteId { get; set; }

        public int UserId { get; set; }

        public int Choice { get; set; }

        public VotedEventArgs(string voteId, int userId, int choice)
        {
            this.VoteId = voteId;
            this.UserId = userId;
            this.Choice = choice;
        }
    }
}
