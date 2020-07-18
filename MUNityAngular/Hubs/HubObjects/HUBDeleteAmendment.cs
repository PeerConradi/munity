using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.Resolution;

namespace MUNityAngular.Hubs.HubObjects
{

    /// <summary>
    /// The HUBDeleteAmendment is a one on one copy of the DeleteAmendmentModel.
    /// The only difference is:
    /// It only stores Data and has absolutly no logic. We need to create this class
    /// because SignalR will otherwise send Data that is tagged with [JsonIgnore].
    /// </summary>
    [Obsolete("When refactoring the Resolution the HubModels should no longer be used")]
    public class HUBDeleteAmendment : HUBAbstractAmendment
    {
        public HUBDeleteAmendment(DeleteAmendmentModel amendment) : base(amendment)
        {
            //No Custom Parameters here
        }

        public HUBDeleteAmendment()
        {

        }
    }
}
