using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Hubs
{

    /// <summary>
    /// This is only a Test and should not be used or changed later
    /// </summary>
    public interface ITypedTestHub
    {
        Task sendToAll(string name, string message);
    }
}
