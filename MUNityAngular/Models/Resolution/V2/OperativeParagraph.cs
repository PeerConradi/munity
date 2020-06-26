using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Resolution.V2
{
    public class OperativeParagraph : IOperativeParagraph
    {
        public string OperativeParagraphId { get; set; }
        public string Name { get; set; }
        public bool IsLocked { get; set; }
        public bool IsVirtual { get; set; }
        public string Text { get; set; }
        public bool Visible { get; set; }

        public string ActivatedAmendmentId { get; set; }

        public List<OperativeParagraph> Children { get; set; }

        public OperativeParagraph()
        {
            Children = new List<OperativeParagraph>();
            OperativeParagraphId = Util.Tools.IdGenerator.RandomString(36);
        }
    }
}
