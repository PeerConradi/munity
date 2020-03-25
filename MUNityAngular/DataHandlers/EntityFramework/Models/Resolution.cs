using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.EntityFramework.Models
{
    public class Resolution
    {
        public string ResolutionId { get; set; }

        public string Name { get; set; }

        public string OnlineCode { get; set; }

        public bool PublicRead { get; set; }

        public bool PublicWrite { get; set; }

        public bool PublicAmendment { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastChangedDate { get; set; }

        public User CreationUser { get; set; }

        public Resolution()
        {
            ResolutionId = Guid.NewGuid().ToString();
        }
    }
}
