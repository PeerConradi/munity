using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.Resolution;

namespace MUNityAngular.Hubs.HubObjects
{

    /// <summary>
    /// The HUBMoveAmendment is a one on one copy of the MoveAmendmenthModel.
    /// The only difference is:
    /// It only stores Data and has absolutly no logic. We need to create this class
    /// because SignalR will otherwise send Data that is tagged with [JsonIgnore].
    /// </summary>
    public class HUBMoveAmendment : HUBAbstractAmendment
    {
        public int NewPosition { get; set; }

        public string NewSectionID { get; set; }

        public HUBMoveAmendment(MoveAmendment amendment) : base (amendment)
        {
            this.NewPosition = amendment.NewPosition;
            this.NewSectionID = amendment.NewSectionID;
        }

        public HUBMoveAmendment()
        {

        }
    }
}
