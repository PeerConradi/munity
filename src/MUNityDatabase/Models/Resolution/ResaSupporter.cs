using System;
using MUNity.Database.Models.Conference;
using MUNity.Database.Models.Conference.Roles;

namespace MUNity.Database.Models.Resolution;

public class ResaSupporter
{
    public string ResaSupporterId { get; set; }

    public string Name { get; set; }

    public Participation Participation { get; set; }

    public ConferenceDelegateRole Role { get; set; }

    public ResaElement Resolution { get; set; }

    public ResaSupporter()
    {
        this.ResaSupporterId = Guid.NewGuid().ToString();
    }
}
