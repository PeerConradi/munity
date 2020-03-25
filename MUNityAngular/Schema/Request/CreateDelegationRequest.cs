using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Schema.Request
{
    public class CreateDelegationRequest
    {

        public string Name { get; set; }

        public string FullName { get; set; }

        public string Abbreviation { get; set; }

        public string Type { get; set; }
    }
}
