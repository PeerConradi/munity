using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Session;

public class AttendanceState
{
    public enum AttendanceTypes
    {
        Ok,
        Late,
        Replacement,
        NotChecked
    }

    public long AttendanceStateId { get; set; }

    public AttendanceCheck AttendanceCheck { get; set; }

    public DateTime AttendTime { get; set; }

    public AttendanceTypes Type { get; set; } = AttendanceTypes.NotChecked;

    /// <summary>
    /// use either Role or Participant. If the user has a munity account
    /// rather use the Participant of not and you have some sort of anonymous conference
    /// use the Role.
    /// </summary>
    public Conference.Participation Participant { get; set; }

    public Conference.Roles.ConferenceDelegateRole Role { get; set; }

    public string Comment { get; set; }
}
