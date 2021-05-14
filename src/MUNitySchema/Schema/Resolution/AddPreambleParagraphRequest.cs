using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Resolution
{
    public class AddPreambleParagraphRequest : ResolutionRequest
    {
        public AddPreambleParagraphRequest()
        {

        }

        public AddPreambleParagraphRequest(string resolutionId)
        {
            this.ResolutionId = resolutionId;
        }
    }
}
