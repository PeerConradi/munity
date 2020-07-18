using System.Collections.Generic;

namespace MUNityAngular.Models.Resolution.V2
{
    public interface IResolutionHeader
    {
        string Name { get; set; }

        string FullName { get; set; }

        string Topic { get; set; }

        public string AgendaItem { get; set; }

        public string Session { get; set; }

        public string SubmitterName { get; set; }

        public string CommitteeName { get; set; }

        public List<string> Supporters { get; set; }
    }
}