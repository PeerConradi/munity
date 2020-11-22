using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityCore.Models.Conference;
using MUNityCore.Models.Session;

namespace MUNityCore.Services
{
    public class SessionService
    {
        public AttendanceCheck CreateAttendanceCheckForCommittee(Committee committee)
        {
            throw new NotImplementedException();
        }

        public CommitteeSession CreateNewSession(Committee committee)
        {
            var newSession = new CommitteeSession()
            {
                Committee = committee,
                CommitteeSessionId = "sess_ " + Guid.NewGuid().ToString()
            };
            return newSession;
        }
    }
}
