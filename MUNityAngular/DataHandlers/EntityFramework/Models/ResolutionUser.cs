using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.EntityFramework.Models
{
    public class ResolutionUser
    {
        public int ResolutionUserId { get; set; }

        public Resolution Resolution { get; set; }

        public User User { get; set; }

        public DateTime AddedDate { get; set; }

        public bool canRead { get; set; }

        public bool CanEdit { get; set; }
    }
}
