using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Database.Models.Resolution;

public class ResaAddAmendment : ResaAmendment
{
    public override string ResaAmendmentType => "ADD";

    public ResaOperativeParagraph VirtualParagraph { get; set; }

}
