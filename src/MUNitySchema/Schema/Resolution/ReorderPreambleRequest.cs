using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Schema.Resolution
{
    public class ReorderPreambleRequest : ResolutionRequest
    {
        public List<string> NewOrder { get; set; }
    }
}
