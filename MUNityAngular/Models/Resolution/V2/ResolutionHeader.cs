using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Resolution.V2
{
    public class ResolutionHeader : IResolutionHeader
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Topic { get; set; }
        public string AgendaItem { get; set; }
        public string Session { get; set; }
        public string SubmitterName { get; set; }
        public string CommitteeName { get; set; }

        public List<string> Supporters { get; set; }

        public ResolutionHeader()
        {
            Supporters = new List<string>();
        }
    }
}
