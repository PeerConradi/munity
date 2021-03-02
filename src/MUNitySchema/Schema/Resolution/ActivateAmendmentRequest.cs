using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Resolution
{
    public class ActivateAmendmentRequest : ResolutionRequest
    {
        public string AmendmentId { get; set; }
    }
}
