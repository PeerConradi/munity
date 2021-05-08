using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUNityCore.Models.User
{
    public class MunityRole : IdentityRole<string>
    {

        public MunityRole()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
