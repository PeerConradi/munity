using MUNity.Database.Models.Conference.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Session
{
    public class AttendanceCheck
    {
        public string AttendanceId { get; set; }

        public DateTime AttendanceDate { get; set; }

        public List<AbstractConferenceRole> ShouldAttendRoles { get; set; }

        public List<Attendance> Attendances { get; set; }
    }
}
