using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Resolution
{
    public class ChangeOperativeParagraphTextRequest : ResolutionRequest
    {
        public string OperativeParagraphId { get; set; }

        public string NewText { get; set; }
    }
}
