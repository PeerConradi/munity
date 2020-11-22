using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Session
{
    public class Attendance
    {
        public enum AttendanceTypes
        {
            Ok,
            Late,
            Replacement
        }

        public int AttendanceId { get; set; }

        public AttendanceCheck AttendanceCheck { get; set; }

        public DateTime AttendTime { get; set; }

        public AttendanceTypes Type { get; set; }

        public string Comment { get; set; }
    }
}
