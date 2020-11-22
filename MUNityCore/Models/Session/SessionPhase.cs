using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityCore.Models.Conference;

namespace MUNityCore.Models.Session
{
    public class SessionPhase
    {
        public string SessionPhaseId { get; set; }

        public string SessionPhaseName { get; set; }

        public DateTime SessionPhaseStart { get; set; }

        public DateTime SessionPhaseEnd { get; set; }

        public Committee SessionCommittee { get; set; }
    }
}
