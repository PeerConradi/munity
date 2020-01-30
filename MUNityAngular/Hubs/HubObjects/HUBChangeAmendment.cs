using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Hubs.HubObjects
{
    public class HUBChangeAmendment : HUBAbstractAmendment
    {
        public string NewText { get; set; }


        public HUBChangeAmendment(Models.ChangeAmendmentModel amendment) : base(amendment)
        {
            this.NewText = amendment.NewText;
        }
    }
}
