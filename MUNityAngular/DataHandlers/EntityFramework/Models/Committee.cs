using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.EntityFramework.Models
{
    public class Committee
    {
        public string CommitteeId { get; set; }

        public string Name { get; set; }

        public string FullName { get; set; }

        public Committee()
        {
            CommitteeId = Guid.NewGuid().ToString();
        }
    }
}
