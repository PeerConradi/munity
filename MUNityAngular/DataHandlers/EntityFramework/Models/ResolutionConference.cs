using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.EntityFramework.Models
{
    public class ResolutionConference
    {

        public Committee Committee { get; set; }

        public Conference Conference { get; set; }

        public Resolution Resolution { get; set; }

    }
}
