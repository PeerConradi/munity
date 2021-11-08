using MUNity.Database.Models.Conference.Roles;

namespace MUNity.Database.Models.Conference;

public class DelegationApplicationPickedDelegationUserWish
{
    public long DelegationApplicationPickedDelegationUserWishId { get; set; }

    public DelegationApplicationPickedDelegation DelegationApplicationPickedDelegation { get; set; }


    public DelegationApplicationUserEntry UserEntry { get; set; }

    public ConferenceDelegateRole Role { get; set; }

    public byte Prio { get; set; }

    public string Comment { get; set; }
}
