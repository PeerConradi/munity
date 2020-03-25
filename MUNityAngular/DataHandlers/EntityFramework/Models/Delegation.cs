using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.EntityFramework.Models
{
    public class Delegation
    {
        public string DelegationId { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public string Abbreviation { get; set; }

        public string Type { get; set; }

        public string IconName { get; set; }

        public Delegation()
        {
            DelegationId = Guid.NewGuid().ToString();
        }
    }
}
