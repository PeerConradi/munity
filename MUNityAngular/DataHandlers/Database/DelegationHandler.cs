using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.Database
{
    public class DelegationHandler
    {
        private static Models.DelegationModel _delegationAF = new Models.DelegationModel() { ID = "default_af", Name = "Afghanistan", ISO = "AF" };
        private static Models.DelegationModel _delegationEG = new Models.DelegationModel() { ID = "default_eg", Name = "Ägypten", ISO = "EQ" };
        private static Models.DelegationModel _delegationAL = new Models.DelegationModel() { ID = "default_al", Name = "Albanien", ISO = "AL" };
        private static List<Models.DelegationModel> _defaultDelegations;

        public static Models.DelegationModel GetDelegation(string id)
        {
            var inDefault = AllDefaultDelegations().FirstOrDefault(n => n.ID == id);
            if (inDefault != null) return inDefault;

            return null;
        }

        public static List<Models.DelegationModel> AllDefaultDelegations()
        {
            if (_defaultDelegations == null)
            {
                _defaultDelegations = new List<Models.DelegationModel>();
                _defaultDelegations.Add(_delegationAF);
                _defaultDelegations.Add(_delegationEG);
                _defaultDelegations.Add(_delegationAL);
            }
            return _defaultDelegations;
        }


    }
}
