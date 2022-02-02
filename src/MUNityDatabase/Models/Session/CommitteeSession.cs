using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Simulation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Session;

public class CommitteeSession
{
    [MaxLength(80)]
    public string CommitteeSessionId { get; set; }

    public string Name { get; set; }

    public Committee Committee { get; set; }

    public AttendanceCheck AttendanceCheck { get; set; }

    public ICollection<ProtocolEntry> ProtocolEntries { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public ICollection<SessionVoting> Votings { get; set; }

    public ICollection<AgendaItem> AgendaItems { get; set; }

    public CommitteeSession()
    {
        this.CommitteeSessionId = Guid.NewGuid().ToString();
    }
}
