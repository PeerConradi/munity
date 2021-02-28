using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Resolution.SqlResa
{
    public class ResaMoveAmendment : ResaAmendment
    {
        public override string ResaAmendmentType => "MOVE";

        public ResaOperativeParagraph SourceParagraph { get; set; }

        public ResaOperativeParagraph VirtualParagraph { get; set; }
    }
}
