using MUNity.Database.Models.Conference.Roles;
using System;

namespace MUNity.Database.Models.Resolution
{
    public interface IResaAmendmentSupporter
    {
        ResaAmendment Amendment { get; set; }
        string ResaAmendmentSupporterId { get; set; }
        ConferenceDelegateRole Role { get; set; }
        DateTimeOffset SupportTimestamp { get; set; }
    }
}