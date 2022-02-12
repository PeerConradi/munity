using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Session;

public class SessionPresents
{
    public int SessionPresentsId { get; set; }

    public CommitteeSession Session { get; set; }

    public DateTime CreatedTime { get; set; }

    public DateTime? CheckedDate { get; set; } = null;

    public ICollection<PresentsState> CheckedUsers { get; set; }

    public bool MarkedFinished { get; set; }

    public SessionPresents()
    {
        CreatedTime = DateTime.Now;
    }
}
