using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Resolution.V2
{
    public class ResolutionUser
    {
        public int ResolutionUserId { get; set; }

        public int CoreUserId { get; set; }

        public string Username { get; set; }

        public string ForeName { get; set; }

        public string LastName { get; set; }

    }
}
