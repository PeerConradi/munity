using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityCore.Models.Conference;

namespace MUNityCore.Models.Session
{
    public class AttendanceCheck
    {
        public string AttendanceId { get; set; }

        public DateTime AttendanceDate { get; set; }

        public List<AbstractConferenceRole> ShouldAttendRoles { get; set; }

        public List<Attendance> Attendances { get; set; }
    }
}
