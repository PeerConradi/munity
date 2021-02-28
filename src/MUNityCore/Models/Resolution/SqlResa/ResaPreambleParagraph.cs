using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Resolution.SqlResa
{
    public class ResaPreambleParagraph
    {
        public string ResaPreambleParagraphId { get; set; }

        public string Text { get; set; }

        public bool IsLocked { get; set; }

        public bool IsCorrected { get; set; }

        public string Comment { get; set; }

        public ResaElement ResaElement { get; set; }

        public ResaPreambleParagraph()
        {
            this.ResaPreambleParagraphId = Guid.NewGuid().ToString();
        }
    }
}
