using MUNityAngular.DataHandlers.EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.SimSim
{
    public class SimSimUser : ISimSimUserFacade
    {
        public string UserToken { get; set; }

        public string HiddenToken { get; set; }

        public string DisplayName { get; set; }

        public Delegation Delegation { get; set; }

        public bool IsChair { get; set; }

        public SimSimUser()
        {
            UserToken = Guid.NewGuid().ToString();
            HiddenToken = Guid.NewGuid().ToString();
        }
    }
}
