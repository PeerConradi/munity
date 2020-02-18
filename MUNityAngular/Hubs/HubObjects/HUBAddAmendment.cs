using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.Resolution;

namespace MUNityAngular.Hubs.HubObjects
{

    /// <summary>
    /// The HUBAddAmendment is a one on one copy of the AddAmendmentModel.
    /// The only difference is:
    /// It only stores Data and has absolutly no logic. We need to create this class
    /// because SignalR will otherwise send Data that is tagged with [JsonIgnore].
    /// </summary>
    public class HUBAddAmendment : HUBAbstractAmendment
    {
        public int TargetPosition { get; set; }

        public string NewText { get; set; }

        public string TargetResolutionID { get; set; }

        public HUBAddAmendment(AddAmendmentModel amendment) : base(amendment)
        {
            this.TargetPosition = amendment.TargetPosition;
            this.NewText = amendment.NewText;
            this.TargetResolutionID = amendment.TargetResolutionID;
        }

        public HUBAddAmendment()
        {

        }
    }
}
