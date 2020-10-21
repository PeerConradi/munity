using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Schema.Request
{
    public class GrandUserResolutionRights
    {
        public string ResolutionId { get; set; }

        public string Username { get; set; }

        public bool CanRead { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }
    }
}
