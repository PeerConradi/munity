﻿using MUNity.Database.Models.Conference.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Session;

public class AttendanceCheck
{
    public long AttendanceCheckId { get; set; }

    public DateTime AttendanceDate { get; set; }

    public ICollection<AttendanceState> Attendances { get; set; }
}
