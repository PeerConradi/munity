using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.Database
{
    public interface IDatabaseHandler
    {
        DatabaseInformation.ETableStatus TableStatus { get; }

        bool CreateTables();
    }
}
