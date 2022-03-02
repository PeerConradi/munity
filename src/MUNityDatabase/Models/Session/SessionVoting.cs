using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Session;

public class SessionVoting
{
    public string SessionVotingId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public Session.CommitteeSession Session { get; set; }

    public ICollection<SessionVotingSlot> VoteSlots { get; set; }

    public bool IsActive { get; set; }

    public bool AllowAbstention { get; set; }

   public VotingStates State { get; set; }

    public SessionVoting()
    {
        SessionVotingId = Guid.NewGuid().ToString();
    }
}

public enum VotingStates
{
    Creating,
    Open,
    Finished,
    Deleted
}