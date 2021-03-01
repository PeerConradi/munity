using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Resolution.SqlResa
{
    public class ResaChangeAmendment : ResaAmendment
    {
        public override string ResaAmendmentType => "CHANGE";

        public ResaOperativeParagraph TargetParagraph { get; set; }

        public string NewText { get; set; }

        
    }
}
