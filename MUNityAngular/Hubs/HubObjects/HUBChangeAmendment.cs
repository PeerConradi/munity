using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.Resolution;

namespace MUNityAngular.Hubs.HubObjects
{

    /// <summary>
    /// The HUBChangeAmendment is a one on one copy of the ChangeAmendmentModel.
    /// The only difference is:
    /// It only stores Data and has absolutly no logic. We need to create this class
    /// because SignalR will otherwise send Data that is tagged with [JsonIgnore].
    /// </summary>
    public class HUBChangeAmendment : HUBAbstractAmendment
    {
        public string NewText { get; set; }


        public HUBChangeAmendment(ChangeAmendmentModel amendment) : base(amendment)
        {
            this.NewText = amendment.NewText;
        }

        public HUBChangeAmendment()
        {

        }
    }
}
