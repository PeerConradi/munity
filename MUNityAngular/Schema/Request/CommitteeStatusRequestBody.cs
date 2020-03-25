using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Schema.Request
{
    public class CommitteeStatusRequestBody
    {
        public string CommitteeId { get; set; }

        public string Status { get; set; }

        public string AgendaItem { get; set; }
    }
}
