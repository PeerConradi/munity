using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.Models.Conference
{

    /// <summary>
    /// A State is a country like the USA, Germany etc.
    /// </summary>
    public class State
    {
        public int StateId { get; set; }

        public string StateName { get; set; }

        public string StateFullName { get; set; }

        public string StateIso { get; set; }
    }
}
