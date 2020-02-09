using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.Resolution;

namespace MUNityAngular.Hubs.HubObjects
{
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
