using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Simulation.Resolution
{
    public class ResolutionSmallInfo
    {

        public string ResolutionId { get; set; }

        public DateTime LastChangedTime {get; set; }

        public string Name { get; set; }

        public bool AllowPublicEdit { get; set; }

        public bool AllowAmendments { get; set; }
    }
}
