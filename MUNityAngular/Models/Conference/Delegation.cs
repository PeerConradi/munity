using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference
{
    public class Delegation
    {
        public string DelegationId { get; set; }

        public string Name { get; set; }

        public Conference Conference { get; set; }

        public Delegation()
        {
            DelegationId = Guid.NewGuid().ToString();
        }
    }
}
