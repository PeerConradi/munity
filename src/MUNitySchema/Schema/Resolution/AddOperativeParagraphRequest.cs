﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Resolution
{
    public class AddOperativeParagraphRequest : ResolutionRequest
    {
        public AddOperativeParagraphRequest()
        {

        }

        public AddOperativeParagraphRequest(string resolutionId)
        {
            this.ResolutionId = resolutionId;
        }
    }
}
