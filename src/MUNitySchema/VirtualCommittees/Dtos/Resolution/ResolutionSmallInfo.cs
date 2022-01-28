using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.VirtualCommittees.Dtos.Resolution
{
    public class ResolutionSmallInfo
    {

        public string ResolutionId { get; set; }

        public DateTime LastChangedTime {get; set; }

        public string Name { get; set; }

        public bool AllowPublicEdit { get; set; }

        public bool AllowAmendments { get; set; }

        public string SubmitterName { get; set; }

        public int PreambleParagraphCount { get; set; }

        public int OperativeParagraphCount { get; set; }

        public int AmendmentCount { get; set; }
    }
}
