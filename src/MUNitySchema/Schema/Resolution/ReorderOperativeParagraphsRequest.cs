using System.Collections.Generic;

namespace MUNity.Schema.Resolution
{
    public class ReorderOperativeParagraphsRequest : ResolutionRequest
    {
        public List<string> NewOrder { get; set; }
    }
}
