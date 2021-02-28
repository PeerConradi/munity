using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Resolution.SqlResa
{
    public class ResaElement
    {
        public string ResaElementId { get; set; }

        public string Topic { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string AgendaItem { get; set; }

        public string Session { get; set; }

        public string SubmitterName { get; set; }

        public string CommitteeName { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<ResaPreambleParagraph> PreambleParagraphs { get; set; }



        public ResaElement()
        {
            this.ResaElementId = Guid.NewGuid().ToString();
        }
    }
}
