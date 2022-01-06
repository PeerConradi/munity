using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.VirtualCommittee.Dtos
{
    /// <summary>
    /// The target audience for the voting.
    /// </summary>
    public enum EVotingMode
    {
        /// <summary>
        /// Everyone whom is online can vote
        /// </summary>
        Everyone,
        /// <summary>
        /// All Participants (Delegates, Guests, and NGOs) can vote
        /// </summary>
        AllParticipants,
        /// <summary>
        /// Only the delegates that are online can vote
        /// </summary>
        JustDelegates,
        /// <summary>
        /// Only the guests can vote
        /// </summary>
        JustGuests,
        /// <summary>
        /// Only the Non-Government-Organizations can vote.
        /// </summary>
        JustNgos
    }
}
