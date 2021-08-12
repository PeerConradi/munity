using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNity.Database.Models.User
{
    public class MunityRole : IdentityRole<string>
    {

        public bool CanCreateNewOrganizations { get; set; }

        public MunityRole()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
