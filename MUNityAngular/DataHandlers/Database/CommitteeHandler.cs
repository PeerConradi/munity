using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.Database
{
    public class CommitteeHandler
    {
        public static Models.CommitteeModel GetCommittee(string id)
        {
            if (id == TestCommittee.ID)
                return TestCommittee;

            return null;
        }

        public static Models.CommitteeModel TestCommittee
        {
            get
            {
                var committee = new Models.CommitteeModel();
                committee.ID = "test_committee";
                committee.Name = "Test Committee";
                committee.FullName = "A Committee to Test";
                committee.Abbreviation = "Ctt";
                committee.Article = "the";
                DelegationHandler.AllDefaultDelegations().ToList().ForEach(n => committee.AddDelegation(n));
                return committee;
            }
        }
    }
}
