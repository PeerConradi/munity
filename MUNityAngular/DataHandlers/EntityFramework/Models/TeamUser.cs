using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.EntityFramework.Models
{
    public class TeamUser
    {
        public int TeamUserId { get; set; }

        public TeamRole Role { get; set; }

        public User User { get; set; }
    }
}
