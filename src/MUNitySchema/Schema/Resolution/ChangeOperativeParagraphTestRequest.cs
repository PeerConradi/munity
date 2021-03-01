using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Resolution
{
    public class ChangeOperativeParagraphTestRequest : ResolutionRequest
    {
        public string OperativeParagraphId { get; set; }

        public string NewText { get; set; }
    }
}
