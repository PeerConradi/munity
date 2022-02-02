using MUNity.Base;
using MUNity.Database.Models.Conference.Roles;
using System;

namespace MUNity.Database.Models.Simulation;

public class SessionVotingSlot
{
    public int SessionVotingSlotId { get; set; }

    public SessionVoting Voting { get; set; }

    public ConferenceDelegateRole User { get; set; }

    public EVoteStates Choice { get; set; }

    public DateTime? VoteTime { get; set; }
}
