using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Dtos.Resolutions
{
    public class CreateAmendmentPattern
    {
        public string ResolutionId { get; set; }

        public EAmendmentTypes AmendmentType { get; set; }

        public string ParagraphId { get; set; } = string.Empty;

        public string NewValue { get; set; } = "";

        public int NewIndex { get; set; }

        
        public string SubmitterName { get; set; } = "";

        public CreateAmendmentPattern(string resolutionId)
        {
            this.ResolutionId = resolutionId;
        }

        public CreateAmendmentPattern()
        {

        }
    }
}
