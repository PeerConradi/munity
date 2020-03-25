using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityAngular.DataHandlers.EntityFramework.Models
{
    public class UserAuths
    {
        public int UserAuthsId { get; set; }

        public User User { get; set; }

        public bool CanCreateConference { get; set; }

        //public bool CanCreateBlog { get; set; }
    }
}
