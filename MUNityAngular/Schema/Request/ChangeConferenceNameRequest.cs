using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Schema.Request
{
    public class ChangeConferenceNameRequest
    {
        public string ConferenceID { get; set; }

        public string NewName { get; set; }
    }
}
