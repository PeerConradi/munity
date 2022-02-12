using MUNity.Database.Models.Conference.Roles;
using System;

namespace MUNity.Database.Models.Session;

public class PresentsState
{
    public enum PresentsStates
    {
        NotChecked,
        Present,
        Absent,
        Late
    }

    public int PresentsStateId { get; set; }

    public SessionPresents SessionPresents { get; set; }

    public ConferenceDelegateRole Role{ get; set; }

    public PresentsStates State { get; set; }

    public string StateValue { get; set; }

    public DateTime? RegistertTimestamp { get; set; }
}
