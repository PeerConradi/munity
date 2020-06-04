using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.Conference;

namespace MUNityAngular.DataHandlers.EntityFramework.Models
{
    public class ResolutionConference
    {
        public int ResolutionConferenceId { get; set; }

        public Committee Committee { get; set; }

        public Conference Conference { get; set; }

        public Resolution Resolution { get; set; }

    }
}
