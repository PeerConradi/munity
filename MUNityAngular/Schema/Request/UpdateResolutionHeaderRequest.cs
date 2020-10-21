using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Schema.Request
{
    public class UpdateResolutionHeaderRequest
    {
        public string ResolutionId { get; set; }

        public string Title { get; set; }

        public string SubmitterName { get; set; }

        public List<string> Supporters { get; set; }

        public string Committee { get; set; }
    }
}
