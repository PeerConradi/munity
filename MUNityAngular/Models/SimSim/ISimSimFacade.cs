using MUNityCore.DataHandlers.EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.SimSim
{
    public interface ISimSimFacade
    {

        int SimSimId { get; set; }
        string Name { get; set; }

        List<ISimSimUserFacade> Users { get; set; }

        public bool UsingPassword { get; }

        public bool CanJoin { get; set; }
    }
}
