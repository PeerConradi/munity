using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUNityAngular.Models.Resolution;

namespace MUNityAngular.Hubs.HubObjects
{
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
