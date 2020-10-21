using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Resolution.V2
{
    public interface IOperativeSection
    {
        public string OperativeSectionId { get; set; }

        List<OperativeParagraph> Paragraphs { get; set; }

        List<ChangeAmendment> ChangeAmendments { get; set; }

        List<AddAmendment> AddAmendments { get; set; }

        List<MoveAmendment> MoveAmendments { get; set; }

        List<DeleteAmendment> DeleteAmendments { get; set; }
    }
}
