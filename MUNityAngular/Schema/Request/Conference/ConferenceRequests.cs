using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Schema.Request.Conference
{
    public class ConferenceRequests
    {
        public class ChangeConferenceName
        {
            [Required]
            public string ConferenceId { get; set; }

            [Required]
            public string NewName { get; set; }
        }
    }
}
