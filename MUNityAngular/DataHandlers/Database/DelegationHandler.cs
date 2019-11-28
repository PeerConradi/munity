using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.Database
{
    public class DelegationHandler
    {
        public static Models.DelegationModel GetDelegation(string id)
        {
            var inDefault = AllDefaultDelegations().FirstOrDefault(n => n.ID == id);
            if (inDefault != null) return inDefault;

            return null;
        }

        public static List<Models.DelegationModel> AllDefaultDelegations()
        {
            var list = new List<Models.DelegationModel>();
            list.Add(new Models.DelegationModel() { ID="default_af", Name = "Afghanistan", ISO="AF" });
            list.Add(new Models.DelegationModel() { ID="default_eg", Name = "Ägypten", ISO = "EQ" });
            list.Add(new Models.DelegationModel() { ID = "default_al", Name = "Albanien", ISO = "AL" });
            return list;
        }


    }
}
