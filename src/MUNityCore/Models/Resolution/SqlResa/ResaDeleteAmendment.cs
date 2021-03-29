using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.Resolution.SqlResa
{
    public class ResaDeleteAmendment : ResaAmendment
    {

        public override string ResaAmendmentType => "DELETE";

        public ResaOperativeParagraph TargetParagraph { get; set; }

        public ResaDeleteAmendment()
        {
            this.ResaAmendmentId = Guid.NewGuid().ToString();
        }
    }
}
