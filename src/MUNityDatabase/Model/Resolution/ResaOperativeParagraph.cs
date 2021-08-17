using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Resolution
{
    public class ResaOperativeParagraph
    {
        public string ResaOperativeParagraphId { get; set; }

        public string Name { get; set; } = "";

        public string Text { get; set; } = "";

        public bool IsLocked { get; set; } = false;

        public bool IsVirtual { get; set; } = false;

        public bool Visible { get; set; } = true;

        public bool Corrected { get; set; } = false;

        public List<ResaOperativeParagraph> Children { get; set; }

        public string Comment { get; set; } = "";

        public int OrderIndex { get; set; }

        public ResaElement Resolution { get; set; }

        public ResaOperativeParagraph Parent { get; set; }

        public List<ResaDeleteAmendment> DeleteAmendments { get; set; }
        public List<ResaChangeAmendment> ChangeAmendments { get; set; }

        public List<ResaMoveAmendment> MoveAmendments { get; set; }

        public ResaOperativeParagraph()
        {
            ResaOperativeParagraphId = Guid.NewGuid().ToString();
        }
    }
}
