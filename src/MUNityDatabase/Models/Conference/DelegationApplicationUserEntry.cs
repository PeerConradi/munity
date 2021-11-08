using MUNity.Database.Models.User;
using MUNityBase;

namespace MUNity.Database.Models.Conference;

public class DelegationApplicationUserEntry
{
    public int DelegationApplicationUserEntryId { get; set; }

    public DelegationApplication Application { get; set; }

    public MunityUser User { get; set; }

    public DelegationApplicationUserEntryStatuses Status { get; set; }

    public bool CanWrite { get; set; }

    public string Message { get; set; }
}
