using MUNity.Database.Models.Conference;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Session
{
    public class CommitteeSession
    {
        [MaxLength(80)]
        public string CommitteeSessionId { get; set; }

        public Committee Committee { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
