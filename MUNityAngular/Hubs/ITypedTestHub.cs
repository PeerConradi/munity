using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Hubs
{
    public interface ITypedTestHub
    {
        Task sendToAll(string name, string message);
    }
}
