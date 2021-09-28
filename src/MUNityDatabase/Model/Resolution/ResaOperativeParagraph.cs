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

        public ICollection<ResaOperativeParagraph> Children { get; set; }

        public string Comment { get; set; } = "";

        public int OrderIndex { get; set; }

        public ResaElement Resolution { get; set; }

        public ResaOperativeParagraph Parent { get; set; }

        public ICollection<ResaDeleteAmendment> DeleteAmendments { get; set; }
        public ICollection<ResaChangeAmendment> ChangeAmendments { get; set; }

        public ICollection<ResaMoveAmendment> MoveAmendments { get; set; }

        public ResaOperativeParagraph()
        {
            ResaOperativeParagraphId = Guid.NewGuid().ToString();
            Children = new List<ResaOperativeParagraph>();
            DeleteAmendments = new List<ResaDeleteAmendment>();
            ChangeAmendments = new List<ResaChangeAmendment>();
            MoveAmendments = new List<ResaMoveAmendment>();
        }
    }
}
