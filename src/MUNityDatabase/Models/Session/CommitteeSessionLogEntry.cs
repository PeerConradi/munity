using System;

namespace MUNity.Database.Models.Session;

public class CommitteeSessionLogEntry
{
    public int CommitteeSessionLogEntryId { get; set; }

    public CommitteeSession CommitteeSession { get; set; }

    public DateTime Timestamp { get; set; }

    public string Provider { get; set; }

    public string Name { get; set; }

    public string Text { get; set; }
}