using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Resolution
{
    public class ChangePreambleParagraphTextRequest : ResolutionRequest
    {
        public string PreambleParagraphId { get; set; }

        public string NewText { get; set; }
    }

    public class RemovePreambleParagraphRequest : ResolutionRequest
    {
        public string PreambleParagraphId { get; set; }
    }
}
