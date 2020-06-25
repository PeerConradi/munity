using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Resolution.V2
{
    public interface IOperativeParagraph
    {
        string OperativeParagraphId { get; set; }

        string Name { get; set; }

        bool IsLocked { get; set; }

        bool IsVirtual { get; set; }

        string Text { get; set; }

        bool Visible { get; set; }
    }
}
