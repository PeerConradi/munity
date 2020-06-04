using MUNityAngular.DataHandlers.EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.Conference;

namespace MUNityAngular.Models.SimSim
{
    public interface ISimSimUserFacade
    {
        const string USER_ROLE_DELEGATION = "Delegation";

        const string USER_ROLE_SPECTATOR = "Spectator";

        const string USER_ROLE_CHAIRMAN = "Chairman";

        static IEnumerable<string> POSSIBLE_ROLES
        {
            get
            {
                yield return USER_ROLE_DELEGATION;
                yield return USER_ROLE_CHAIRMAN;
                yield return USER_ROLE_SPECTATOR;
            }
        }

        string UserToken { get; set; }

        string DisplayName { get; set; }

        Delegation Delegation { get; set; }

        bool IsChair { get; set; }

        public string Role { get; set; }
    }
}
