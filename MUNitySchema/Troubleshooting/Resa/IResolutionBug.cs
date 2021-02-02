using MUNity.Models.Resolution;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUNity.Troubleshooting.Resa
{
    public interface IResolutionBug
    {
        string Description { get; }

        bool Detect(Resolution resolution);

        bool Fix(Resolution resolution);
    }
}
