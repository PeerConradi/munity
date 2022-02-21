using MUNity.Database.Models.Conference.Roles;
using System;
using System.ComponentModel.DataAnnotations;

namespace MUNity.Database.Models.Resolution;

public class ResaAmendmentSupporter : IResaAmendmentSupporter
{
    [Key]
    public string ResaAmendmentSupporterId { get; set; }

    public ResaAmendment Amendment { get; set; }

    public DateTimeOffset SupportTimestamp { get; set; }

    public ConferenceDelegateRole Role { get; set; }

    public ResaAmendmentSupporter()
    {
        this.ResaAmendmentSupporterId = Guid.NewGuid().ToString();
    }
}