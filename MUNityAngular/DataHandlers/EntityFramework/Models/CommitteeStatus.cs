using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.Conference;

namespace MUNityAngular.DataHandlers.EntityFramework.Models
{
    public class CommitteeStatus
    {
        public string CommitteeStatusId { get; set; }

        public Committee Committee { get; set; }

        public string Status { get; set; }

        public string AgendaItem { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
