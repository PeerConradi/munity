using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUNity.Database.Models.User;

public class MunityUserRole : IdentityUserRole<string>
{
    public virtual MunityUser User { get; set; }

    public virtual MunityRole Role { get; set; }

    public MunityUserRole()
    {

    }
}
