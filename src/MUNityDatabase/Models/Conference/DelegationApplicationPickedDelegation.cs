using System.Collections.Generic;

namespace MUNity.Database.Models.Conference;

public class DelegationApplicationPickedDelegation
{
    public int DelegationApplicationPickedDelegationId { get; set; }

    public DelegationApplication Application { get; set; }

    public byte Priority { get; set; }

    public Delegation Delegation { get; set; }

    public ICollection<DelegationApplicationPickedDelegationUserWish> UserWishes { get; set; }

    public string Comment { get; set; }
}
