using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Hubs.HubObjects
{
    public class HUBDeleteAmendment : HUBAbstractAmendment
    {
        public HUBDeleteAmendment(Models.DeleteAmendmentModel amendment) : base(amendment)
        {
            //No Custom Parameters here
        }
    }
}
