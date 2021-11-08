using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Session;

public class ProtocolEntry
{
    public int ProtocolEntryId { get; set; }

    public CommitteeSession Session { get; set; }

    public DateTime ProtocolTime { get; set; }

    public string ProtocolType { get; set; }

    public string ProtocolName { get; set; }

    public string ProtocolText { get; set; }
}
